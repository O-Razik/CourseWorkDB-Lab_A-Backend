namespace Lab_A.Abstraction.IModels;

public interface IInventoryInOrder
{
    public int InventoryInOrderId { get; set; }
    
    public double? Price { get; set; }
    
    public int? Quantity { get; set; }
    
    public int? InventoryId { get; set; }
    
    public int? InventoryOrderId { get; set; }

    public IInventory Inventory { get; set; }

    public ICollection<IInventoryDelivery> InventoryDeliveries { get; set; }

    public IInventoryOrder InventoryOrder { get; set; }
}