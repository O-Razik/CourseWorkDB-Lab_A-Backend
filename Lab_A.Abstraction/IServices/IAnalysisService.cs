using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAnalysisService
{
    Task<IAnalysis> CreateAsync(IAnalysis entity);
    Task<IAnalysis?> ReadAsync(int id);
    Task<IEnumerable<IAnalysis>> ReadAllAsync();
    Task<IAnalysis?> UpdateAsync(IAnalysis entity);
    Task<bool> DeleteAsync(int id);
}