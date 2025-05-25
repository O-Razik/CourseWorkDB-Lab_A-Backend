using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IOrderAnalysisRepository : ICrud<IOrderAnalysis>
{
    Task<IEnumerable<IOrderAnalysis>> ReadAllByClientOrderAsync(int clientOrderId);
}