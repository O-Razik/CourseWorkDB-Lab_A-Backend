namespace Lab_A.BLL.Dto;

public class InventoryOrderDto
{
    public int InventoryOrderId { get; set; }
    public int Number { get; set; }
    public int SupplierId { get; set; }
    public int StatusId { get; set; }
    public double Fullprice { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<InventoryInOrderDto> InventoryInOrders { get; set; } = new List<InventoryInOrderDto>();
    public StatusDto? Status { get; set; } = null!;
    public SupplierDto Supplier { get; set; } = null!;
}