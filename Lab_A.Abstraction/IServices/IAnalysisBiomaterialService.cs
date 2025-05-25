using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAnalysisBiomaterialService
{
    Task<IAnalysisBiomaterial> CreateAsync(IAnalysisBiomaterial entity);
    Task<IAnalysisBiomaterial?> ReadAsync(int id);
    Task<IEnumerable<IAnalysisBiomaterial>> ReadAllAsync();
    Task<IEnumerable<IAnalysisBiomaterial>> ReadAllByAnalysisIdAsync(int analysisId);
    Task<IAnalysisBiomaterial?> UpdateAsync(IAnalysisBiomaterial entity);
    Task<bool> DeleteAsync(int id);
}