namespace Lab_A.BLL.Dto;

public class InventoryDeliveryDto
{
    public int InventoryDeliveryId { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTime DeliveryDate { get; set; }
   
    public int StatusId { get; set; }
    
    public int InventoryInLaboratoryId { get; set; }
    
    public int InventoryInOrderId { get; set; }

    public string LaboratoryFullAddress { get; set; } = null!;

    public DateTime? ExpirationDate { get; set; }
    
    public virtual InventoryInLaboratoryDto? InventoryInLaboratory { get; set; } = null!;

    public virtual InventoryInOrderDto InventoryInOrder { get; set; } = null!;
    
    public virtual StatusDto? Status { get; set; } = null!;
}