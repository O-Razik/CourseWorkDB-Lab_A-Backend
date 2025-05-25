using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class AnalysisCenterMapper
{
    public static AnalysisCenterDto ToDto(this IAnalysisCenter analysisCenter)
    {
        return new AnalysisCenterDto()
        {
            AnalysisCenterId = analysisCenter.AnalysisCenterId,
            CityId = analysisCenter.CityId,
            Address = analysisCenter.Address,
            City = analysisCenter.City.ToDto()
        };
    }
    public static IAnalysisCenter ToEntity(this AnalysisCenterDto analysisCenterDto)
    {
        return new AnalysisCenter
        {
            AnalysisCenterId = analysisCenterDto.AnalysisCenterId,
            CityId = analysisCenterDto.CityId,
            Address = analysisCenterDto.Address,
            City = (City)analysisCenterDto.City.ToEntity()
        };
    }
}