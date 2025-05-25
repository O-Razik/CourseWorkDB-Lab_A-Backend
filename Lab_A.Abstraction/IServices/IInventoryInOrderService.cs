using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IInventoryInOrderService
{
    Task<IInventoryInOrder> CreateAsync(IInventoryInOrder entity);
    Task<IInventoryInOrder?> ReadAsync(int id);
    Task<IEnumerable<IInventoryInOrder>> ReadAllAsync();
    Task<IEnumerable<IInventoryInOrder>> ReadAllByInventoryOrderIdAsync(int inventoryOrderId);
    Task<IInventoryInOrder?> UpdateAsync(IInventoryInOrder entity);
    Task<bool> DeleteAsync(int id);
}