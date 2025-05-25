using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.Services;

public class BiomaterialDeliveryService : IBiomaterialDeliveryService
{
    private readonly IBiomaterialDeliveryRepository _repository;

    public BiomaterialDeliveryService(IBiomaterialDeliveryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IBiomaterialDelivery> CreateAsync(IBiomaterialDelivery entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IBiomaterialDelivery?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IBiomaterialDelivery>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? analysisCenterId = null,
        int? statusId = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();

        var pipeline = new Pipeline<IBiomaterialDelivery>();

        // Date range filtering for delivery date
        if (fromDate.HasValue || toDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IBiomaterialDelivery>(
                fromDate,
                toDate,
                nameof(BiomaterialDelivery.DeliveryDate)));
        }

        // Analysis Center filtering
        if (analysisCenterId.HasValue)
        {
            pipeline.Register(new EqualityStep<IBiomaterialDelivery, int?>(
                analysisCenterId,
                nameof(BiomaterialDelivery.AnalysisCenterId)));
        }

        // Status filtering
        if (statusId.HasValue)
        {
            pipeline.Register(new EqualityStep<IBiomaterialDelivery, int?>(
                statusId,
                nameof(BiomaterialDelivery.StatusId)));
        }

        // Apply all filters first
        var filteredQuery = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IBiomaterialDelivery>(pageNumber, pageSize)
            .Process(filteredQuery);

        return pagedQuery.ToList();
    }

    public async Task<IBiomaterialDelivery?> UpdateAsync(IBiomaterialDelivery entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}