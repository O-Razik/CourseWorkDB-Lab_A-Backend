using Lab_A.Abstraction.IPipeline;

namespace Lab_A.BLL.Pipeline;

public class PagingStep<T> : IPipelineStep<T> where T : class
{
    private readonly int _pageNumber;
    private readonly int _pageSize;

    public PagingStep(int pageNumber, int pageSize)
    {
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public IQueryable<T> Process(IQueryable<T> input)
    {
        return input.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize);
    }
}