namespace Lab_A.Abstraction.IModels;

public interface IAnalysisResult
{
    public int AnalysisResultId { get; set; }
    
    public int? OrderAnalysisId { get; set; }
    
    public int? Indicator { get; set; }
    
    public DateTime? ExecutionDate { get; set; }
    
    public string Description { get; set; }

    public int? AnalysisCenterId { get; set; }

    public IAnalysisCenter AnalysisCenter { get; set; }

    public IOrderAnalysis OrderAnalysis { get; set; }
}