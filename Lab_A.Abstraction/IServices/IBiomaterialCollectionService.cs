using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IBiomaterialCollectionService
{
    Task<IBiomaterialCollection> CreateAsync(IBiomaterialCollection entity);
    Task<IBiomaterialCollection?> ReadAsync(int id);
    Task<IEnumerable<IBiomaterialCollection>> ReadAllAsync(
        DateTime? fromExpirationDate = null,
        DateTime? toExpirationDate = null,
        DateTime? fromCollectionDate = null,
        DateTime? toCollectionDate = null,
        int? laboratoryId = null,
        int? inventoryId = null,
        int? biomaterialId = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10,
        bool notDelivered = false
        );
    Task<IBiomaterialCollection?> UpdateAsync(IBiomaterialCollection entity);
    Task<bool> DeleteAsync(int id);
}