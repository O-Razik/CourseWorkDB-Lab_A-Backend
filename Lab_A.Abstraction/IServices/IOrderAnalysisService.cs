using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IOrderAnalysisService
{
    Task<IOrderAnalysis> CreateAsync(IOrderAnalysis entity);
    Task<IOrderAnalysis?> ReadAsync(int id);
    Task<IEnumerable<IOrderAnalysis>> ReadAllAsync();
    Task<IEnumerable<IOrderAnalysis>> ReadAllByClientOrderIdAsync(int clientOrderId);
    Task<IOrderAnalysis?> UpdateAsync(IOrderAnalysis entity);
    Task<bool> DeleteAsync(int id);
}