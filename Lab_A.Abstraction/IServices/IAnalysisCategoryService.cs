using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAnalysisCategoryService
{
    Task<IAnalysisCategory> CreateAsync(IAnalysisCategory entity);
    Task<IAnalysisCategory?> ReadAsync(int id);
    Task<IEnumerable<IAnalysisCategory>> ReadAllAsync();
    Task<IAnalysisCategory?> UpdateAsync(IAnalysisCategory entity);
    Task<bool> DeleteAsync(int id);
}