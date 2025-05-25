using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;

    public SupplierService(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<ISupplier> CreateAsync(ISupplier entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<ISupplier?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<ISupplier>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<ISupplier?> UpdateAsync(ISupplier entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}