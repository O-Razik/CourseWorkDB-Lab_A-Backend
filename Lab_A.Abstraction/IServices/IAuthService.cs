using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IAuthService<T> where T : class
{
    Task<T> RegisterAsync(string email, string password);

    Task<T?> LoginAsync(string email, string password);

    string GenerateJwtToken(IUserEmployee user);
}