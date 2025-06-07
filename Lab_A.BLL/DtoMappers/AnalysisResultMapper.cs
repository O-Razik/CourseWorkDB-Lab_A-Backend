using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class AnalysisResultMapper
{
    public static AnalysisResultDto ToDto(this IAnalysisResult analysisResult)
    {
        return new AnalysisResultDto()
        {
            AnalysisResultId = analysisResult.AnalysisResultId,
            OrderAnalysisId = analysisResult.OrderAnalysisId,
            ExecutionDate = analysisResult.ExecutionDate,
            Indicator = analysisResult.Indicator,
            Description = analysisResult.Description,
            AnalysisCenterId = analysisResult.AnalysisCenterId,
            OrderAnalysis = analysisResult.OrderAnalysis.ToDto2(),
            AnalysisCenter = analysisResult.AnalysisCenter.ToDto(),
        };
    }

    public static IAnalysisResult ToEntity(this AnalysisResultDto analysisResultDto)
    {
        return new AnalysisResult()
        {
            AnalysisResultId = analysisResultDto.AnalysisResultId,
            OrderAnalysisId = analysisResultDto.OrderAnalysisId,
            ExecutionDate = analysisResultDto.ExecutionDate,
            Indicator = analysisResultDto.Indicator,
            Description = analysisResultDto.Description,
            AnalysisCenterId = analysisResultDto.AnalysisCenterId,
            OrderAnalysis = (OrderAnalysis)analysisResultDto.OrderAnalysis.ToEntity(),
            AnalysisCenter = (AnalysisCenter)analysisResultDto.AnalysisCenter.ToEntity(),
        };
    }
}