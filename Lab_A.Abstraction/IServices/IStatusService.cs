using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IStatusService
{
    Task<IStatus> CreateAsync(IStatus entity);
    Task<IStatus?> ReadAsync(int id);
    Task<IEnumerable<IStatus>> ReadAllAsync();
    Task<IStatus?> UpdateAsync(IStatus entity);
    Task<bool> DeleteAsync(int id);
}