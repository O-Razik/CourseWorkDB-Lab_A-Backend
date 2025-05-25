using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAnalysisCenterService
{
    Task<IAnalysisCenter> CreateAsync(IAnalysisCenter entity);
    Task<IAnalysisCenter?> ReadAsync(int id);
    Task<IEnumerable<IAnalysisCenter>> ReadAllAsync();
    Task<IAnalysisCenter?> UpdateAsync(IAnalysisCenter entity);
    Task<bool> DeleteAsync(int id);
}