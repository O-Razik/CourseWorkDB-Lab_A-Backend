using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class InventoryInOrderService : IInventoryInOrderService
{
    private readonly IInventoryInOrderRepository _repository;

    public InventoryInOrderService(IInventoryInOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IInventoryInOrder> CreateAsync(IInventoryInOrder entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IInventoryInOrder?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IInventoryInOrder>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IEnumerable<IInventoryInOrder>> ReadAllByInventoryOrderIdAsync(int inventoryOrderId)
    {
        return await _repository.ReadAllByInventoryOrderIdAsync(inventoryOrderId);
    }

    public async Task<IInventoryInOrder?> UpdateAsync(IInventoryInOrder entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}