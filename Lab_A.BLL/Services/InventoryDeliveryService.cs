using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class InventoryDeliveryService : IInventoryDeliveryService
{
    private readonly IInventoryDeliveryRepository _repository;
    private readonly IInventoryOrderRepository _orderRepository;

    public InventoryDeliveryService(IInventoryDeliveryRepository repository, IInventoryOrderRepository orderRepository)
    {
        _repository = repository;
        _orderRepository  = orderRepository;
    }

    public async Task<IInventoryDelivery> CreateAsync(IInventoryDelivery entity)
    {
        var result = await _repository.CreateAsync(entity);
        if (result.InventoryInOrder.InventoryOrderId.HasValue)
        {
            await _orderRepository.UpdateStatusAsync(result.InventoryInOrder.InventoryOrderId.Value);
        }
        return result;
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
        var result = await _repository.UpdateAsync(entity);
        if (result!.InventoryInOrder.InventoryOrderId.HasValue)
        {
            await _orderRepository.UpdateStatusAsync(result.InventoryInOrder.InventoryOrderId.Value);
        }
        return result;
    }

    public async Task<IInventoryDelivery?> UpdateStatusAsync(int deliveryId, int status)
    {
        var result = await _repository.UpdateStatusAsync(deliveryId, status);
        if (result!.InventoryInOrder.InventoryOrderId.HasValue)
        {
            await _orderRepository.UpdateStatusAsync(result.InventoryInOrder.InventoryOrderId.Value);
        }
        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}