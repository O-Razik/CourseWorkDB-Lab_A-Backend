using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class LaboratoryScheduleService : ILaboratoryScheduleService
{
    private readonly ILaboratoryScheduleRepository _repository;

    public LaboratoryScheduleService(ILaboratoryScheduleRepository repository)
    {
        _repository = repository;
    }

    public async Task<ILaboratorySchedule> CreateAsync(ILaboratorySchedule entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<ILaboratorySchedule?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<ILaboratorySchedule>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IEnumerable<ILaboratorySchedule>> ReadByLaboratoryAsync(int laboratoryId)
    {
        return await _repository.ReadByLaboratoryAsync(laboratoryId);
    }

    public async Task<ILaboratorySchedule?> UpdateAsync(ILaboratorySchedule entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}