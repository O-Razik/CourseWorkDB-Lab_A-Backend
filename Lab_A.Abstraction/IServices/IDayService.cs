using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IDayService
{
    Task<IDay> CreateAsync(IDay entity);
    Task<IDay?> ReadAsync(int id);
    Task<IEnumerable<IDay>> ReadAllAsync();
    Task<IDay?> UpdateAsync(IDay entity);
    Task<bool> DeleteAsync(int id);
}