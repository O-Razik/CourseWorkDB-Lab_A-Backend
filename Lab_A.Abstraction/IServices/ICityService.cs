using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface ICityService
{
    Task<ICity> CreateAsync(ICity entity);
    Task<ICity?> ReadAsync(int id);
    Task<IEnumerable<ICity>> ReadAllAsync();
    Task<ICity?> UpdateAsync(ICity entity);
    Task<bool> DeleteAsync(int id);
}