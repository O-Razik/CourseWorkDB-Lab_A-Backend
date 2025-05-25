namespace Lab_A.Abstraction.IModels;

public interface IAnalysisBiomaterial
{
    public int AnalysisBiomaterialId { get; set; }
    
    public int? AnalysisId { get; set; }
    
    public int? BiomaterialId { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public IAnalysis Analysis { get; set; }

    public IBiomaterial Biomaterial { get; set; }
}