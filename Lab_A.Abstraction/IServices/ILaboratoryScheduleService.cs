using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface ILaboratoryScheduleService
{
    Task<ILaboratorySchedule> CreateAsync(ILaboratorySchedule entity);
    Task<ILaboratorySchedule?> ReadAsync(int id);
    Task<IEnumerable<ILaboratorySchedule>> ReadAllAsync();
    Task<IEnumerable<ILaboratorySchedule>> ReadByLaboratoryAsync(int laboratoryId);
    Task<ILaboratorySchedule?> UpdateAsync(ILaboratorySchedule entity);
    Task<bool> DeleteAsync(int id);
}