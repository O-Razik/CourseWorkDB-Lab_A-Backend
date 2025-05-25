using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class AnalysisMapper
{
    public static AnalysisDto ToDto(this IAnalysis analysis)
    {
        return new AnalysisDto
        {
            AnalysisId = analysis.AnalysisId,
            CategoryId = analysis.CategoryId,
            Name = analysis.Name,
            Description = analysis.Description,
            Price = analysis.Price,
            AnalysisBiomaterials = analysis.AnalysisBiomaterials.Select(ab => ab.ToDto()).ToList(),
            Category = analysis.Category.ToDto(),
        };
    }


    public static IAnalysis ToEntity(this AnalysisDto analysisDto)
    {
        return new Analysis
        {
            AnalysisId = analysisDto.AnalysisId,
            CategoryId = analysisDto.CategoryId,
            Name = analysisDto.Name,
            Description = analysisDto.Description,
            Price = analysisDto.Price,
            AnalysisBiomaterials = analysisDto.AnalysisBiomaterials.Select(ab => (AnalysisBiomaterial)ab.ToEntity()).ToList(),
            Category = (AnalysisCategory)analysisDto.Category!.ToEntity(),
        };
    }
}