namespace Lab_A.BLL.Dto;

public class BiomaterialDeliveryDto
{
    public int BiomaterialDeliveryId { get; set; }
    
    public int BiomaterialCollectionId { get; set; }
    
    public int AnalysisCenterId { get; set; }
    
    public int StatusId { get; set; }
    
    public DateTime DeliveryDate { get; set; }
    
    public AnalysisCenterDto AnalysisCenter { get; set; } = null!;

    public BiomaterialCollectionDto BiomaterialCollection { get; set; } = null!;
    
    public StatusDto Status { get; set; } = null!;
}