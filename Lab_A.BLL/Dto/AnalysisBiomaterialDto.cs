#nullable disable

namespace Lab_A.BLL.Dto;

public class AnalysisBiomaterialDto
{
    public int AnalysisBiomaterialId { get; set; }
    
    public int? AnalysisId { get; set; }
    
    public int? BiomaterialId { get; set; }

    public virtual BiomaterialDto Biomaterial { get; set; }
}