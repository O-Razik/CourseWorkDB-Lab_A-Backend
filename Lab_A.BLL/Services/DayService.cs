using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class DayService : IDayService
{
    private readonly IDayRepository _repository;

    public DayService(IDayRepository repository)
    {
        _repository = repository;
    }

    public async Task<IDay> CreateAsync(IDay entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IDay?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IDay>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IDay?> UpdateAsync(IDay entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}