using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface ISupplierService
{
    Task<ISupplier> CreateAsync(ISupplier entity);
    Task<ISupplier?> ReadAsync(int id);
    Task<IEnumerable<ISupplier>> ReadAllAsync();
    Task<ISupplier?> UpdateAsync(ISupplier entity);
    Task<bool> DeleteAsync(int id);
}
