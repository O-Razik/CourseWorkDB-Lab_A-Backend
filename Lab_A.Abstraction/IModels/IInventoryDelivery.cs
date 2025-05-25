namespace Lab_A.Abstraction.IModels;

public interface IInventoryDelivery
{
    public int InventoryDeliveryId { get; set; }
    
    public int? Quantity { get; set; }
    
    public DateTime? DeliveryDate { get; set; }
    
    public int StatusId { get; set; }
    
    public int? InventoryInLaboratoryId { get; set; }
    
    public int? InventoryInOrderId { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public IInventoryInLaboratory InventoryInLaboratory { get; set; }
    
    public IInventoryInOrder InventoryInOrder { get; set; }
    
    public IStatus Status { get; set; }
}