namespace Lab_A.Abstraction.IModels;

public interface IAnalysis
{
    public int AnalysisId { get; set; }

    public string Name { get; set; }
    
    public int CategoryId { get; set; }
    
    public string Description { get; set; }
    
    public double? Price { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public ICollection<IAnalysisBiomaterial> AnalysisBiomaterials { get; set; }
    
    public IAnalysisCategory Category { get; set; }
    
    public ICollection<IOrderAnalysis> OrderAnalyses { get; set; }
}