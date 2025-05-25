using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IAnalysisCenterRepository : ICrud<IAnalysisCenter>
{
    Task<IAnalysisCenter?> GetByCity(int cityId);
}