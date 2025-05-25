using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IInventoryInLaboratoryService
{
    Task<IInventoryInLaboratory> CreateAsync(IInventoryInLaboratory entity);
    Task<IInventoryInLaboratory?> ReadAsync(int id);
    Task<IEnumerable<IInventoryInLaboratory>> ReadAllAsync();
    Task<IEnumerable<IInventoryInLaboratory>> ReadAllByLaboratoryAsync(int laboratoryId, bool isZero);
    Task<IInventoryInLaboratory?> UpdateAsync(IInventoryInLaboratory entity);
    Task<bool> DeleteAsync(int id);
}