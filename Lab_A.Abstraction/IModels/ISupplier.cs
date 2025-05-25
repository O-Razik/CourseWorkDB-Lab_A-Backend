namespace Lab_A.Abstraction.IModels;

public interface ISupplier
{
    
    public int SupplierId { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string License { get; set; }

    public ICollection<IInventoryOrder> InventoryOrders { get; set; }
}