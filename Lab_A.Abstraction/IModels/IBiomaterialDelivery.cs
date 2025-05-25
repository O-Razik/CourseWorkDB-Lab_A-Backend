namespace Lab_A.Abstraction.IModels;

public interface IBiomaterialDelivery
{
    public int BiomaterialDeliveryId { get; set; }
    
    public int? BiomaterialCollectionId { get; set; }
    
    public int? AnalysisCenterId { get; set; }
    
    public int StatusId { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public IAnalysisCenter AnalysisCenter { get; set; }

    public IBiomaterialCollection BiomaterialCollection { get; set; }

    public IStatus Status { get; set; }
}