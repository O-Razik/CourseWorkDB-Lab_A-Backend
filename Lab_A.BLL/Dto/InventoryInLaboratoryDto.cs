namespace Lab_A.BLL.Dto
{
    public class InventoryInLaboratoryDto
    {
        public int InventoryInLaboratoryId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Quantity { get; set; }
        public int InventoryId { get; set; }
        public int LaboratoryId { get; set; }
        public InventoryDto Inventory { get; set; } = null!;
    }
}