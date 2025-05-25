namespace Lab_A.BLL.Dto;

public class SupplierDto
{
    public int SupplierId { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Email { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
    
    public string License { get; set; } = null!;
}