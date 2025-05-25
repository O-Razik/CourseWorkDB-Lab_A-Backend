namespace Lab_A.Abstraction.IModels;

public interface IInventoryInLaboratory
{
    public int InventoryInLaboratoryId { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public int? Quantity { get; set; }
    
    public int? InventoryId { get; set; }
    
    public int? LaboratoryId { get; set; }

    public ICollection<IBiomaterialCollection> BiomaterialCollections { get; set; }

    public IInventory Inventory { get; set; }

    public ICollection<IInventoryDelivery> InventoryDeliveries { get; set; }

    public ILaboratory Laboratory { get; set; }
}