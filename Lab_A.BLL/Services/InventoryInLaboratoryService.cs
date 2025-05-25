using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class InventoryInLaboratoryService : IInventoryInLaboratoryService
{
    private readonly IInventoryInLaboratoryRepository _repository;

    public InventoryInLaboratoryService(IInventoryInLaboratoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IInventoryInLaboratory> CreateAsync(IInventoryInLaboratory entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IInventoryInLaboratory?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IInventoryInLaboratory>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IEnumerable<IInventoryInLaboratory>> ReadAllByLaboratoryAsync(int laboratoryId, bool isZero)
    {
        var result = await _repository.GetByLaboratoryAsync(laboratoryId);

        if (!isZero)
        {
            result = result.Where(iil => iil.Quantity > 0);
        }

        return result;
    }

    public async Task<IInventoryInLaboratory?> UpdateAsync(IInventoryInLaboratory entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}