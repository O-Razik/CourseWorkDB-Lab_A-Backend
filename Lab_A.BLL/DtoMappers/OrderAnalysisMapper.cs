using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class OrderAnalysisMapper
{
    public static OrderAnalysisDto ToDto(this IOrderAnalysis orderAnalysis)
    {
        return new OrderAnalysisDto()
        {
            OrderAnalysisId = orderAnalysis.OrderAnalysisId,
            ClientOrderId = (int)orderAnalysis.ClientOrderId!,
            AnalysisId = (int)orderAnalysis.AnalysisId!,
            Analysis = orderAnalysis.Analysis.ToDto(),
            ClientOrder = orderAnalysis.ClientOrder.ToDto2(),
            AnalysisResults = orderAnalysis.AnalysisResults?.Select(x => x.ToDto()).ToList(),
        };
    }
    
    public static OrderAnalysisDto ToDto2(this IOrderAnalysis orderAnalysis)
    {
        return new OrderAnalysisDto()
        {
            OrderAnalysisId = orderAnalysis.OrderAnalysisId,
            ClientOrderId = (int)orderAnalysis.ClientOrderId!,
            AnalysisId = (int)orderAnalysis.AnalysisId!,
            Analysis = orderAnalysis.Analysis.ToDto(),
            ClientOrder = orderAnalysis.ClientOrder.ToDto2(),
        };
    }
    
    public static IOrderAnalysis ToEntity(this OrderAnalysisDto orderAnalysisDto)
    {
        return new OrderAnalysis()
        {
            OrderAnalysisId = orderAnalysisDto.OrderAnalysisId,
            ClientOrderId = orderAnalysisDto.ClientOrderId,
            AnalysisId = orderAnalysisDto.AnalysisId,
            Analysis = (Analysis)orderAnalysisDto.Analysis.ToEntity(),
        };
    }
}