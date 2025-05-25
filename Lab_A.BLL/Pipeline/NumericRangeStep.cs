using System.Linq.Expressions;
using System.Reflection;
using Lab_A.Abstraction.IPipeline;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lab_A.BLL.Pipeline;

public class NumericRangeStep<T> : IPipelineStep<T> where T : class
{
    private readonly double? _minValue;
    private readonly double? _maxValue;
    private readonly string _propertyName;

    public NumericRangeStep(double? minValue, double? maxValue, string propertyName)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        _propertyName = propertyName;
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, _propertyName);

        // Convert property to nullable if it's not already
        var propertyAsNullable = Expression.Convert(property, typeof(double?));

        var minCondition = _minValue.HasValue
            ? Expression.GreaterThanOrEqual(propertyAsNullable, Expression.Constant(_minValue, typeof(double?)))
            : null;

        var maxCondition = _maxValue.HasValue
            ? Expression.LessThanOrEqual(propertyAsNullable, Expression.Constant(_maxValue, typeof(double?)))
            : null;

        Expression? combinedCondition = null;

        if (minCondition != null && maxCondition != null)
        {
            combinedCondition = Expression.AndAlso(minCondition, maxCondition);
        }
        else if (minCondition != null)
        {
            combinedCondition = minCondition;
        }
        else if (maxCondition != null)
        {
            combinedCondition = maxCondition;
        }

        if (combinedCondition == null)
        {
            return input;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(combinedCondition, parameter);
        return input.Where(lambda);
    }

}