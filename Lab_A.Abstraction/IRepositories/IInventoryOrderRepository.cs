using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IInventoryOrderRepository : ICrud<IInventoryOrder>
{
    Task<IInventoryOrder?> CancelOrderAsync(int id);
    
    Task<IInventoryOrder?> UpdateStatusAsync(int id);
}