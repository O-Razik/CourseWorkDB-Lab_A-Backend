using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IScheduleService
{
    Task<ISchedule> CreateAsync(ISchedule entity);
    Task<ISchedule?> ReadAsync(int id);
    Task<IEnumerable<ISchedule>> ReadAllAsync();
    Task<ISchedule?> UpdateAsync(ISchedule entity);
    Task<bool> DeleteAsync(int id);
}