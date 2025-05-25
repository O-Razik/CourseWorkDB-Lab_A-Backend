using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class AnalysisCategoryMapper
{
    public static AnalysisCategoryDto ToDto(this IAnalysisCategory analysisCategory)
    {
        return new AnalysisCategoryDto()
        {
            AnalysisCategoryId = analysisCategory.AnalysisCategoryId,
            Category = analysisCategory.Category,
        };
    }

    public static IAnalysisCategory ToEntity(this AnalysisCategoryDto analysisCategoryDto)
    {
        return new AnalysisCategory()
        {
            AnalysisCategoryId = analysisCategoryDto.AnalysisCategoryId,
            Category = analysisCategoryDto.Category,
        };
    }
}