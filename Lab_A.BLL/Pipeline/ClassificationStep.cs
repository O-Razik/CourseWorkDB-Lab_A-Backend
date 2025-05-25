using System.Linq.Expressions;
using Lab_A.Abstraction.IPipeline;

namespace Lab_A.BLL.Pipeline;

public class ClassificationStep<T> : IPipelineStep<T> where T : class
{
    private readonly IEnumerable<int> _includedValues;
    private readonly string _propertyName;

    public ClassificationStep(IEnumerable<int> includedValues, string propertyName)
    {
        _includedValues = includedValues?.ToList() ?? new List<int>();
        _propertyName = propertyName;
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        if (_includedValues == null || !_includedValues.Any())
            return input;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, _propertyName);
        var containsMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(int));

        var constant = Expression.Constant(_includedValues);
        var containsCall = Expression.Call(containsMethod, constant, property);
        var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);

        return input.Where(lambda);
    }
}