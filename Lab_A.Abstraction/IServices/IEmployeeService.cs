using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IEmployeeService
{
    Task<IEmployee> CreateAsync(IEmployee entity);
    Task<IEmployee?> ReadAsync(int id);
    Task<IEnumerable<IEmployee>> ReadAllAsync(
        string? search = null,
        int? laboratoryId = null,
        List<int>? positionIds = null,
        int pageNumber = 1,
        int pageSize = 10);
    Task<IEmployee?> UpdateAsync(IEmployee entity);
    Task<bool> DeleteAsync(int id);
}