using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;

    public ScheduleService(IScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ISchedule> CreateAsync(ISchedule entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<ISchedule?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<ISchedule>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<ISchedule?> UpdateAsync(ISchedule entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}