using System.Linq.Expressions;
using System.Reflection;
using Lab_A.Abstraction.IPipeline;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.BLL.Pipeline;

public class StringContainsStep<T> : IPipelineStep<T> where T : class
{
    private readonly string _searchTerm;
    private readonly string[] _propertyNames;

    public StringContainsStep(string searchTerm, params string[] propertyNames)
    {
        _searchTerm = searchTerm?.Trim().ToLower() ?? string.Empty;
        _propertyNames = propertyNames ?? Array.Empty<string>();
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        if (string.IsNullOrWhiteSpace(_searchTerm) || _propertyNames.Length == 0)
            return input;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyChecks = new List<Expression>();

        // Get required method info once
        var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes);
        var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        var concatMethod = typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string) });

        foreach (var propertyName in _propertyNames)
        {
            var property = GetNestedProperty(parameter, propertyName);
            if (property == null || property.Type != typeof(string)) continue;

            // Null check
            var nullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(string)));

            // EF.Functions.Like (preferred for database queries)
            var efFunctions = typeof(EF).GetMethod(nameof(EF.Functions))?.ReturnType;
            var likeMethod = efFunctions?.GetMethod(nameof(DbFunctionsExtensions.Like),
                new[] { efFunctions, typeof(string), typeof(string) });

            if (likeMethod != null)
            {
                var functions = Expression.Property(null, typeof(EF).GetProperty(nameof(EF.Functions)));
                var likeCall = Expression.Call(
                    null,
                    likeMethod,
                    functions,
                    property,
                    Expression.Constant($"%{_searchTerm}%"));

                var condition = Expression.AndAlso(nullCheck, likeCall);
                propertyChecks.Add(condition);
            }
            else
            {
                // Fallback for non-EF providers
                var toLowerCall = Expression.Call(property, toLowerMethod);
                var containsCall = Expression.Call(toLowerCall, containsMethod, Expression.Constant(_searchTerm));
                var condition = Expression.AndAlso(nullCheck, containsCall);
                propertyChecks.Add(condition);
            }
        }

        // Add full name search if we have both first and last name properties
        if (_propertyNames.Length >= 2 &&
            _propertyNames.Any(p => p.EndsWith("FirstName")) &&
            _propertyNames.Any(p => p.EndsWith("LastName")))
        {
            var firstNameProp = GetNestedProperty(parameter, _propertyNames.First(p => p.EndsWith("FirstName")));
            var lastNameProp = GetNestedProperty(parameter, _propertyNames.First(p => p.EndsWith("LastName")));

            if (firstNameProp != null && lastNameProp != null)
            {
                // Create expression for (FirstName + " " + LastName).Contains(searchTerm)
                var spaceConstant = Expression.Constant(" ");
                var fullNameConcat = Expression.Call(concatMethod, firstNameProp, spaceConstant, lastNameProp);
                var toLowerFullName = Expression.Call(fullNameConcat, toLowerMethod);
                var containsFullName = Expression.Call(toLowerFullName, containsMethod, Expression.Constant(_searchTerm));

                // Null checks for both properties
                var firstNameNotNull = Expression.NotEqual(firstNameProp, Expression.Constant(null, typeof(string)));
                var lastNameNotNull = Expression.NotEqual(lastNameProp, Expression.Constant(null, typeof(string)));
                var bothNotNull = Expression.AndAlso(firstNameNotNull, lastNameNotNull);

                var fullNameCondition = Expression.AndAlso(bothNotNull, containsFullName);
                propertyChecks.Add(fullNameCondition);
            }
        }

        if (propertyChecks.Count == 0)
            return input;

        var orExpression = propertyChecks.Aggregate(Expression.OrElse);
        var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);

        return input.Where(lambda);
    }

    private static MemberExpression? GetNestedProperty(ParameterExpression parameter, string propertyPath)
    {
        Expression currentExpression = parameter;
        foreach (var part in propertyPath.Split('.'))
        {
            var property = currentExpression.Type.GetProperty(part,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return null;

            currentExpression = Expression.Property(currentExpression, property);
        }
        return currentExpression as MemberExpression;
    }
}