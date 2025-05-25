using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _repository;

    public StatusService(IStatusRepository repository)
    {
        _repository = repository;
    }

    public async Task<IStatus> CreateAsync(IStatus entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IStatus?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IStatus>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IStatus?> UpdateAsync(IStatus entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}