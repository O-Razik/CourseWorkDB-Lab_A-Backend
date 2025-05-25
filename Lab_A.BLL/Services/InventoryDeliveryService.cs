using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class InventoryDeliveryService : IInventoryDeliveryService
{
    private readonly IInventoryDeliveryRepository _repository;

    public InventoryDeliveryService(IInventoryDeliveryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IInventoryDelivery> CreateAsync(IInventoryDelivery entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IInventoryDelivery?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IInventoryDelivery>> ReadAllAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? laboratoryId = null,
        IEnumerable<int>? inventoryIds = null,
        IEnumerable<int>? statusIds = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IInventoryDelivery>();

        // Date range filtering
        if (fromDate.HasValue || toDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IInventoryDelivery>(
                fromDate,
                toDate,
                nameof(IInventoryDelivery.DeliveryDate)));
        }

        // Laboratory filtering
        if (laboratoryId != null)
        {
            pipeline.Register(new EqualityStep<IInventoryDelivery, int?>(
                laboratoryId, 
                $"{nameof(IInventoryDelivery.InventoryInLaboratory)}.{nameof(IInventoryInLaboratory.LaboratoryId)}"));
        }

        // Inventory filtering
        if (inventoryIds != null)
        {
            var ids = inventoryIds.ToList();
            if (ids.Any())
            {
                pipeline.Register(new ClassificationStep<IInventoryDelivery>(
                    ids, $"{nameof(IInventoryDelivery.InventoryInOrder)}.{nameof(IInventoryInOrder.InventoryId)}"));
            }
        }

        // Status filtering
        if (statusIds != null)
        {
            var ids = statusIds.ToList();
            if (ids.Any())
            {
                pipeline.Register(new ClassificationStep<IInventoryDelivery>(
                    ids,
                    nameof(IInventoryDelivery.StatusId)));
            }
        }

        // Search filtering
        if (!string.IsNullOrEmpty(search))
        {
            // Search by order number
            // make sure that search has only digits
            if (int.TryParse(search, out var orderNumber))
            {
                pipeline.Register(new EqualityStep<IInventoryDelivery, int?>(
                    orderNumber,
                    $"{nameof(IInventoryDelivery.InventoryInOrder)}.{nameof(IInventoryInOrder.InventoryOrder)}.{nameof(IInventoryOrder.Number)}"));
            }
            else
            {
                pipeline.Register(new StringContainsStep<IInventoryDelivery>(
                    search,
                    $"{nameof(IInventoryDelivery.InventoryInOrder)}.{nameof(IInventoryInOrder.Inventory)}.{nameof(IInventory.InventoryName)}"));
            }
        }

        // Apply all filters first
        query = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IInventoryDelivery>(pageNumber, pageSize)
            .Process(query);

        return pagedQuery.ToList();
    }

    public async Task<IInventoryDelivery?> UpdateAsync(IInventoryDelivery entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}