using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IBiomaterialService
{
    Task<IBiomaterial> CreateAsync(IBiomaterial entity);
    Task<IBiomaterial?> ReadAsync(int id);
    Task<IEnumerable<IBiomaterial>> ReadAllAsync();
    Task<IBiomaterial?> UpdateAsync(IBiomaterial entity);
    Task<bool> DeleteAsync(int id);
}