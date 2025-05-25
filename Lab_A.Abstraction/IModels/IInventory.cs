namespace Lab_A.Abstraction.IModels;

public interface IInventory
{
    public int InventoryId { get; set; }
    
    public string InventoryName { get; set; }
    
    public double? Price { get; set; }

    public ICollection<IInventoryInLaboratory> InventoryInLaboratories { get; set; }

    public ICollection<IInventoryInOrder> InventoryInOrders { get; set; }
}