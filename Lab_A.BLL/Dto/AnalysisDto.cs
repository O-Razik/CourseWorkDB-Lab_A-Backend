namespace Lab_A.BLL.Dto;

public class AnalysisDto
{
    public int AnalysisId { get; set; }
    
    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }
    
    public string Description { get; set; } = null!;

    public double? Price { get; set; }
    
    public virtual ICollection<AnalysisBiomaterialDto> AnalysisBiomaterials { get; set; } = new List<AnalysisBiomaterialDto>();
    
    public virtual AnalysisCategoryDto? Category { get; set; }
}