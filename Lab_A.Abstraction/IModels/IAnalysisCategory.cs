namespace Lab_A.Abstraction.IModels;

public interface IAnalysisCategory
{
    public int AnalysisCategoryId { get; set; }
    
    public string Category { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public ICollection<IAnalysis> Analyses { get; set; }
}