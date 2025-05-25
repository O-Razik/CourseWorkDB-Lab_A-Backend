namespace Lab_A.Abstraction.IModels;

public interface IEmployee
{
    public int EmployeeId { get; set; }
    
    public int PositionId { get; set; }
    
    public int? LaboratoryId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }

    public ICollection<IClientOrder> ClientOrders { get; set; }

    public ILaboratory Laboratory { get; set; }

    public IPosition Position { get; set; }
}