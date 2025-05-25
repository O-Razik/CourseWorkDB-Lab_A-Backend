using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IClientService
{
    Task<IClient> CreateAsync(IClient entity);
    Task<IClient?> ReadAsync(int id);
    Task<IEnumerable<IClient>> ReadAllAsync(
        int? sexId = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<IClient?> UpdateAsync(IClient entity);
    Task<bool> DeleteAsync(int id);
}