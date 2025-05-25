using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IServices;

public interface IBiomaterialDeliveryService
{
    Task<IBiomaterialDelivery> CreateAsync(IBiomaterialDelivery entity);
    Task<IBiomaterialDelivery?> ReadAsync(int id);
    Task<IEnumerable<IBiomaterialDelivery>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? analysisCenterId = null,
        int? statusId = null,
        int pageNumber = 1,
        int pageSize = 10);

    Task<IBiomaterialDelivery?> UpdateAsync(IBiomaterialDelivery entity);
    Task<bool> DeleteAsync(int id);
}