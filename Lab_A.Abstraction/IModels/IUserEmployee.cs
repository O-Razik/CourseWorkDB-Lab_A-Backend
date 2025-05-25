using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Lab_A.Abstraction.IModels;

public interface IUserEmployee
{
    public int UserEmployeeId { get; set; }
    
    public int EmployeeId { get; set; }
    
    public string Email { get; set; }
    
    public byte[] PasswordHash { get; set; }
    
    public byte[] PasswordSalt { get; set; }

    public IEmployee Employee { get; set; }
}