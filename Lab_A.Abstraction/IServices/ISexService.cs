using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface ISexService
{
    Task<ISex> CreateAsync(ISex entity);
    Task<ISex?> ReadAsync(int id);
    Task<IEnumerable<ISex>> ReadAllAsync();
    Task<ISex?> UpdateAsync(ISex entity);
    Task<bool> DeleteAsync(int id);
}