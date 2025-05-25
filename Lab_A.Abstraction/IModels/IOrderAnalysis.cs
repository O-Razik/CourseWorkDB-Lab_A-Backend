namespace Lab_A.Abstraction.IModels;

public interface IOrderAnalysis
{
    public int OrderAnalysisId { get; set; }
    
    public int? AnalysisId { get; set; }
    
    public int? ClientOrderId { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public IAnalysis Analysis { get; set; }

    public ICollection<IAnalysisResult> AnalysisResults { get; set; }

    public IClientOrder ClientOrder { get; set; }
}