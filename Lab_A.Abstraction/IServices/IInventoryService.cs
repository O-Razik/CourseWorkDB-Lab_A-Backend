using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IInventoryService
{
    Task<IInventory> CreateAsync(IInventory entity);
    Task<IInventory?> ReadAsync(int id);
    Task<IEnumerable<IInventory>> ReadAllAsync();
    Task<IInventory?> UpdateAsync(IInventory entity);
    Task<bool> DeleteAsync(int id);
}