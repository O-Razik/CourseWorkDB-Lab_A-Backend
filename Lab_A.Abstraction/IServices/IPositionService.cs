using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IPositionService
{
    Task<IPosition> CreateAsync(IPosition entity);
    Task<IPosition?> ReadAsync(int id);
    Task<IEnumerable<IPosition>> ReadAllAsync();
    Task<IPosition?> UpdateAsync(IPosition entity);
    Task<bool> DeleteAsync(int id);
}