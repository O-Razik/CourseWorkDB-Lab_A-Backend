using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IInventoryInOrderRepository : ICrud<IInventoryInOrder>
{
    Task<IEnumerable<IInventoryInOrder>> ReadAllByInventoryOrderIdAsync(int inventoryOrderId);
}