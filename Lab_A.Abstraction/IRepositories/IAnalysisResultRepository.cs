using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IAnalysisResultRepository : ICrud<IAnalysisResult>
{
    Task<IEnumerable<IAnalysisResult>> GetAnalysisResultsByOrderId(int orderId); 
}