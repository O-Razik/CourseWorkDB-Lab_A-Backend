using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IInventoryOrderService
{
    Task<IInventoryOrder> CreateAsync(IInventoryOrder entity);
    Task<IInventoryOrder?> ReadAsync(int id);
    Task<IEnumerable<IInventoryOrder>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? supplierId = null,
        double? minPrice = null,
        double? maxPrice = null,
        IEnumerable<int>? statusIds = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<IInventoryOrder?> UpdateAsync(IInventoryOrder entity);
    
    Task<IInventoryOrder?> CancelOrderAsync(int id);
    
    Task<bool> DeleteAsync(int id);
}