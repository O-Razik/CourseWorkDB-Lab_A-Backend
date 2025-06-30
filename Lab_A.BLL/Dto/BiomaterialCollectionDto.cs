namespace Lab_A.BLL.Dto;

public class BiomaterialCollectionDto
{
    public int BiomaterialCollectionId { get; set; }
    
    public DateTime ExpirationDate { get; set; }
    
    public double Volume { get; set; }
    
    public DateOnly CollectionDate { get; set; }
    
    public int BiomaterialId { get; set; }
    
    public int ClientOrderId { get; set; }

    public int? ClientOrderNumber { get; set; }

    public int InventoryInLaboratoryId { get; set; }
    
    public BiomaterialDto Biomaterial { get; set; }

    public InventoryInLaboratoryDto? InventoryInLaboratory { get; set; }
}