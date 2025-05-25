using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IUserEmployeeRepository
{
    Task<IUserEmployee> RegisterAsync(string email, string password);

    Task<IUserEmployee?> LoginAsync(string email, string password);

    protected void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

    protected bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
}