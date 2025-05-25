namespace Lab_A.Abstraction.IPipeline;

public interface IPipelineStep<T>
{
    IQueryable<T> Process(IQueryable<T> input);
}