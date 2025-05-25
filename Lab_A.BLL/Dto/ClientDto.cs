using Lab_A.DAL.Models;

namespace Lab_A.BLL.Dto;

public class ClientDto
{
    public int ClientId { get; set; }
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int SexId { get; set; }
    
    public DateOnly Birthdate { get; set; }
    
    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public SexDto Sex { get; set; } = null!;
}