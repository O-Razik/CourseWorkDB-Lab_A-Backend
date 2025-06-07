using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAnalysisResultService
{
    Task<IAnalysisResult> CreateAsync(IAnalysisResult entity);
    Task<IAnalysisResult?> ReadAsync(int id);
    Task<IEnumerable<IAnalysisResult>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? analysisCenterId = null,
        int? analysisId = null,
        string? clientFullname = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<IAnalysisResult?> UpdateAsync(IAnalysisResult entity);
    Task<bool> DeleteAsync(int id);
    Task<Stream> GeneratePdfReportAsync(int analysisResultId);
    Task<IEnumerable<IAnalysisResult>> GetAnalysisResultsByOrderId(int orderId);
}