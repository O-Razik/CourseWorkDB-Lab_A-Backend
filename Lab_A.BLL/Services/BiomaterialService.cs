using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class BiomaterialService : IBiomaterialService
{
    private readonly IBiomaterialRepository _repository;

    public BiomaterialService(IBiomaterialRepository repository)
    {
        _repository = repository;
    }

    public async Task<IBiomaterial> CreateAsync(IBiomaterial entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IBiomaterial?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IBiomaterial>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IBiomaterial?> UpdateAsync(IBiomaterial entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}