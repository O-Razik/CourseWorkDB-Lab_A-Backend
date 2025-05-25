using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IAnalysisBiomaterialRepository : ICrud<IAnalysisBiomaterial>
{
    Task<IEnumerable<IAnalysisBiomaterial>> ReadAllByAnalysisIdAsync(int analysisId);
}