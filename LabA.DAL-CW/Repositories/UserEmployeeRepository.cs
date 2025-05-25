using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class UserEmployeeRepository : IUserEmployeeRepository
{
    private readonly LabAContext _context;

    public UserEmployeeRepository(LabAContext context)
    {
        _context = context;
    }

    public async Task<IUserEmployee> RegisterAsync(string email, string password)
    {
        UserEmployee? existingUser = await _context.UserEmployees
            .Include(x => x.Employee).ThenInclude(x => x.Position)
            .Include(x => x.Employee).ThenInclude(x => x.Laboratory).ThenInclude(x => x.City)
            .SingleOrDefaultAsync(x => x.Email == email);

        if (existingUser != null)
            throw new Exception("User already exists");

        Employee? employee = await _context.Employees.SingleOrDefaultAsync(x => x.Email == email);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        this.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

        var user = new UserEmployee
        {
            EmployeeId = employee.EmployeeId,
            Email = email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.UserEmployees.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<IUserEmployee?> LoginAsync(string email, string password)
    {
        var user = await _context.UserEmployees
            .Include(x => x.Employee).ThenInclude(x => x.Position)
            .Include(x => x.Employee).ThenInclude(x => x.Laboratory).ThenInclude(x => x.City)
            .SingleOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return null;
        return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
}