namespace Lab_A.Abstraction.IModels;

public interface IBiomaterialCollection
{
    public int BiomaterialCollectionId { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public double? Volume { get; set; }
    
    public DateOnly? CollectionDate { get; set; }
    
    public int? BiomaterialId { get; set; }
    
    public int? ClientOrderId { get; set; }
    
    public int? InventoryInLaboratoryId { get; set; }

    public IBiomaterial Biomaterial { get; set; }

    public ICollection<IBiomaterialDelivery> BiomaterialDeliveries { get; set; }

    public IClientOrder ClientOrder { get; set; }

    public IInventoryInLaboratory InventoryInLaboratory { get; set; }
}