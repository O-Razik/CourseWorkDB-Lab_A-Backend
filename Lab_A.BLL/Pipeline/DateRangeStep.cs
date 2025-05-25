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

        Expression? fromCompare = null;
        Expression? toCompare = null;

        if (_fromDate.HasValue)
        {
            var fromValue = Expression.Constant(_fromDate.Value, propertyType);
            fromCompare = Expression.GreaterThanOrEqual(property, fromValue);
        }

        if (_toDate.HasValue)
        {
            var toValue = Expression.Constant(_toDate.Value, propertyType);
            toCompare = Expression.LessThanOrEqual(property, toValue);
        }

        Expression? combined = null;
        if (fromCompare != null && toCompare != null)
        {
            combined = Expression.AndAlso(fromCompare, toCompare);
        }
        else
        {
            combined = fromCompare ?? toCompare;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combined!, parameter);
        return input.Where(lambda);
    }
}