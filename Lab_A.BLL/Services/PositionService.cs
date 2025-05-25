using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class PositionService : IPositionService
{
    private readonly IPositionRepository _repository;

    public PositionService(IPositionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IPosition> CreateAsync(IPosition entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IPosition?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IPosition>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IPosition?> UpdateAsync(IPosition entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}