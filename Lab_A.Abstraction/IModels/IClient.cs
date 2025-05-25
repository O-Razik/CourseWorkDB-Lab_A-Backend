namespace Lab_A.Abstraction.IModels;

public interface IClient
{
    public int ClientId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public int SexId { get; set; }
    
    public DateOnly Birthdate { get; set; }

    public string PhoneNumber { get; set; }
    
    public string Email { get; set; }

    public ICollection<IClientOrder> ClientOrders { get; set; }

    public ISex Sex { get; set; }
}