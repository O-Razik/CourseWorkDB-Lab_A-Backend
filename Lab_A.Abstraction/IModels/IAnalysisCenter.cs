namespace Lab_A.Abstraction.IModels;

public interface IAnalysisCenter
{
    public int AnalysisCenterId { get; set; }
    
    public int CityId { get; set; }

    public string Address { get; set; }
    
    public DateTime? UpdateDatetime { get; set; }
    
    public DateTime CreateDatetime { get; set; }

    public ICollection<IAnalysisResult> AnalysisResults { get; set; }

    public ICollection<IBiomaterialDelivery> BiomaterialDeliveries { get; set; }

    public ICity City { get; set; }
}