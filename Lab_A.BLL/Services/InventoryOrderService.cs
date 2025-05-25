using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class InventoryOrderService : IInventoryOrderService
{
    private readonly IInventoryOrderRepository _repository;

    public InventoryOrderService(IInventoryOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IInventoryOrder> CreateAsync(IInventoryOrder entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IInventoryOrder?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IInventoryOrder>> ReadAllAsync(DateTime? fromDate = null,
        DateTime? toDate = null,
        int? supplierId = null,
        double? minPrice = null,
        double? maxPrice = null,
        IEnumerable<int>? statusIds = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IInventoryOrder>();

        // Date range filtering
        if (fromDate.HasValue || toDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IInventoryOrder>(
                fromDate,
                toDate,
                nameof(IInventoryOrder.OrderDate)));
        }

        // Supplier filtering
        if (supplierId.HasValue)
        {
            pipeline.Register(new EqualityStep<IInventoryOrder, int?>(
                supplierId,
                nameof(IInventoryOrder.SupplierId)));
        }

        // Price range filtering
        if (minPrice.HasValue || maxPrice.HasValue)
        {
            pipeline.Register(new NumericRangeStep<IInventoryOrder>(
                minPrice,
                maxPrice,
                nameof(IInventoryOrder.Fullprice)));
        }

        // Status filtering
        if (statusIds != null)
        {
            var ids = statusIds.ToList();
            if (ids.Any())
            {
                pipeline.Register(new ClassificationStep<IInventoryOrder>(
                    ids,
                    nameof(IInventoryOrder.StatusId)));
            }
        }

        //Search by inventory name
        if(!string.IsNullOrWhiteSpace(search))
        {
            pipeline.Register(new StringContainsStep<IInventoryOrder>(
                search,
                $"{nameof(IInventoryOrder.InventoryInOrders)}.{nameof(IInventoryInOrder.Inventory)}.{nameof(IInventory.InventoryName)}"));
        }

        // Apply all filters first
        var filteredQuery = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IInventoryOrder>(pageNumber, pageSize)
            .Process(filteredQuery);

        return pagedQuery.ToList();
    }

    public async Task<IInventoryOrder?> UpdateAsync(IInventoryOrder entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}