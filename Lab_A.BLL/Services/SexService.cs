using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class SexService : ISexService
{
    private readonly ISexRepository _repository;

    public SexService(ISexRepository repository)
    {
        _repository = repository;
    }

    public async Task<ISex> CreateAsync(ISex entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<ISex?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<ISex>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<ISex?> UpdateAsync(ISex entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}