namespace Lab_A.Abstraction.IPipeline;

public interface IPipeline<T>
{
    IQueryable<T> Execute(IQueryable<T> input);
    IPipeline<T> Register(IPipelineStep<T> step);
}