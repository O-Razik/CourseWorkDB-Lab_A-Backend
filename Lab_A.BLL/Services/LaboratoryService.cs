using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class LaboratoryService : ILaboratoryService
{
    private readonly ILaboratoryRepository _repository;

    public LaboratoryService(ILaboratoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ILaboratory> CreateAsync(ILaboratory entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<ILaboratory?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<ILaboratory>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<ILaboratory?> UpdateAsync(ILaboratory entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}