using System.Linq.Expressions;
using System.Reflection;
using Lab_A.Abstraction.IPipeline;

namespace Lab_A.BLL.Pipeline;

public class DateRangeStep<T> : IPipelineStep<T> where T : class
{
    private readonly DateTime? _fromDate;
    private readonly DateTime? _toDate;
    private readonly string _datePropertyName;

    public DateRangeStep(DateTime? fromDate, DateTime? toDate, string datePropertyName)
    {
        _fromDate = fromDate;
        _toDate = toDate;
        _datePropertyName = datePropertyName;
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        if (!_fromDate.HasValue && !_toDate.HasValue)
            return input;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, _datePropertyName);
        var propertyType = ((PropertyInfo)property.Member).PropertyType;
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        Expression? fromCompare = null;
        Expression? toCompare = null;

        object? fromValueObj = null;
        object? toValueObj = null;

        if (_fromDate.HasValue)
        {
            fromValueObj = underlyingType == typeof(DateOnly)
                ? DateOnly.FromDateTime(_fromDate.Value)
                : Convert.ChangeType(_fromDate.Value, underlyingType);
            var fromValue = Expression.Constant(fromValueObj, propertyType);
            fromCompare = Expression.GreaterThanOrEqual(property, fromValue);
        }

        if (_toDate.HasValue)
        {
            toValueObj = underlyingType == typeof(DateOnly)
                ? DateOnly.FromDateTime(_toDate.Value)
                : Convert.ChangeType(_toDate.Value, underlyingType);
            var toValue = Expression.Constant(toValueObj, propertyType);
            toCompare = Expression.LessThanOrEqual(property, toValue);
        }

        Expression? combined = fromCompare != null && toCompare != null
            ? Expression.AndAlso(fromCompare, toCompare)
            : fromCompare ?? toCompare;

        var lambda = Expression.Lambda<Func<T, bool>>(combined!, parameter);
        return input.Where(lambda);
    }
}