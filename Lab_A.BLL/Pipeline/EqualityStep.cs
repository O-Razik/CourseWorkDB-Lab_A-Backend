using System.Linq.Expressions;
using System;
using Lab_A.Abstraction.IPipeline;

namespace Lab_A.BLL.Pipeline;

public class EqualityStep<T, TValue> : IPipelineStep<T> where T : class
{
    private readonly TValue _value;
    private readonly string _propertyPath;

    public EqualityStep(TValue value, string propertyPath)
    {
        _value = value;
        _propertyPath = propertyPath;
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        if (_value == null)
            return input;

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression property = parameter;

        // Handle nested properties
        foreach (var part in _propertyPath.Split('.'))
        {
            property = Expression.Property(property, part);
        }

        // Handle nullable types
        var constant = Expression.Constant(_value, property.Type);
        var equals = Expression.Equal(property, constant);

        var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
        return input.Where(lambda);
    }
}