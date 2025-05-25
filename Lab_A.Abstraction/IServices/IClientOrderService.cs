using Lab_A.Abstraction.IData;
using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IClientOrderService
{
    Task<IClientOrder> CreateAsync(IClientOrder entity);
    Task<IClientOrder?> ReadAsync(int id);
    Task<IEnumerable<IClientOrder>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? employeeId = null,
        string? clientFullname = null,
        double? minPrice = null,
        double? maxPrice = null,
        IEnumerable<int>? statusIds = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<IClientOrder?> UpdateAsync(IClientOrder entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<IClientOrder>> ReadAllByClientIdAsync(int clientId);
}