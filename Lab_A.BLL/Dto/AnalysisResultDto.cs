#nullable disable

namespace Lab_A.BLL.Dto;

public class AnalysisResultDto
{
    public int AnalysisResultId { get; set; }
    
    public int? OrderAnalysisId { get; set; }
    
    public int? Indicator { get; set; }
    
    public DateTime? ExecutionDate { get; set; }
    
    public string Description { get; set; }
    
    public int? AnalysisCenterId { get; set; }
    
    public virtual AnalysisCenterDto AnalysisCenter { get; set; }
    
    public virtual OrderAnalysisDto OrderAnalysis { get; set; }
}