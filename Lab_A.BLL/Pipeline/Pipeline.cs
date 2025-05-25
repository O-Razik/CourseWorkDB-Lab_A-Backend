using Lab_A.Abstraction.IPipeline;

namespace Lab_A.BLL.Pipeline;

public class Pipeline<T> : IPipeline<T>
{
    private readonly List<IPipelineStep<T>> _steps = new();

    public IQueryable<T> Execute(IQueryable<T> input)
    {
        return _steps.Aggregate(input, (current, step) => step.Process(current));
    }

    public IPipeline<T> Register(IPipelineStep<T> step)
    {
        _steps.Add(step);
        return this;
    }
}