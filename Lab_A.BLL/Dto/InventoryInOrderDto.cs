using Lab_A.DAL.Models;

namespace Lab_A.BLL.Dto
{
    public class InventoryInOrderDto
    {
        public int InventoryInOrderId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int InventoryOrderId { get; set; }
        public InventoryDto Inventory { get; set; } = null!;
        public ICollection<InventoryDeliveryDto>? InventoryDeliveries { get; set; }
        public int OrderNumber { get; set; }
    }
}