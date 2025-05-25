using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IAnalysisCategoryRepository : ICrud<IAnalysisCategory>
{
    Task<IAnalysisCategory?> GetByName(string? itemCategory);
}