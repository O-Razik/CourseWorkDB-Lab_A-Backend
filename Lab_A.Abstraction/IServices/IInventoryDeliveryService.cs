using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IInventoryDeliveryService
{
    Task<IInventoryDelivery> CreateAsync(IInventoryDelivery entity);
    Task<IInventoryDelivery?> ReadAsync(int id);
    Task<IEnumerable<IInventoryDelivery>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? laboratoryId = null,
        IEnumerable<int>? inventoryIds = null,
        IEnumerable<int>? statusIds = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10
        );
    Task<IInventoryDelivery?> UpdateAsync(IInventoryDelivery entity);
    Task<bool> DeleteAsync(int id);
}