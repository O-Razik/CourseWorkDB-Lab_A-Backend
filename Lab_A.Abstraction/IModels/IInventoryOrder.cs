namespace Lab_A.Abstraction.IModels;

public interface IInventoryOrder
{
    public int InventoryOrderId { get; set; }
    
    public int? Number { get; set; }
    
    public int? SupplierId { get; set; }
    
    public int StatusId { get; set; }

    public double? Fullprice { get; set; }
    
    public DateTime? OrderDate { get; set; }

    public ICollection<IInventoryInOrder> InventoryInOrders { get; set; }

    public IStatus Status { get; set; }

    public ISupplier Supplier { get; set; }
}