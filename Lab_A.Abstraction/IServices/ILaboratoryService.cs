using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface ILaboratoryService
{
    Task<ILaboratory> CreateAsync(ILaboratory entity);
    Task<ILaboratory?> ReadAsync(int id);
    Task<IEnumerable<ILaboratory>> ReadAllAsync();
    Task<ILaboratory?> UpdateAsync(ILaboratory entity);
    Task<bool> DeleteAsync(int id);
}