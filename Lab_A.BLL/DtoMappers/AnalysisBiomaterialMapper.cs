using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class AnalysisBiomaterialMapper
{
    public static AnalysisBiomaterialDto ToDto(this IAnalysisBiomaterial analysisBiomaterial)
    {
        return new AnalysisBiomaterialDto
        {
            AnalysisBiomaterialId = analysisBiomaterial.AnalysisBiomaterialId,
            AnalysisId = analysisBiomaterial.AnalysisId,
            BiomaterialId = analysisBiomaterial.BiomaterialId,
            Biomaterial = analysisBiomaterial.Biomaterial.ToDto()
        };
    }

    public static IAnalysisBiomaterial ToEntity(this AnalysisBiomaterialDto analysisBiomaterialDto)
    {
        return new AnalysisBiomaterial
        {
            AnalysisBiomaterialId = analysisBiomaterialDto.AnalysisBiomaterialId,
            AnalysisId = analysisBiomaterialDto.AnalysisId,
            BiomaterialId = analysisBiomaterialDto.BiomaterialId,
            Biomaterial = (Biomaterial)analysisBiomaterialDto.Biomaterial.ToEntity()
        };
    }
}