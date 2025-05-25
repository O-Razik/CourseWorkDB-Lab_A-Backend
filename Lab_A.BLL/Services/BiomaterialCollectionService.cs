using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.Services;

public class BiomaterialCollectionService : IBiomaterialCollectionService
{
    private readonly IBiomaterialCollectionRepository _repository;

    public BiomaterialCollectionService(IBiomaterialCollectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IBiomaterialCollection> CreateAsync(IBiomaterialCollection entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IBiomaterialCollection?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IBiomaterialCollection>> ReadAllAsync(
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
        bool notDelivered = false)
    {
        var collections = await _repository.ReadAllAsync();

        if (notDelivered)
        {
            collections = collections.Where(x => x.BiomaterialDeliveries.Count == 0);
        }

        var query = collections.AsQueryable();
        var pipeline = new Pipeline<IBiomaterialCollection>();

        // Search filtering
        // Search by order number
        if (!string.IsNullOrEmpty(search))
        {
            if (int.TryParse(search, out var id))
            {
                pipeline.Register(new EqualityStep<IBiomaterialCollection, int?>(
                id,
                    $"{nameof(IBiomaterialCollection.ClientOrder)}.{nameof(IClientOrder.Number)}"));
            }
        }


        // Date range filtering for collection date
        if (fromCollectionDate.HasValue || toCollectionDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IBiomaterialCollection>(
                fromCollectionDate,
                toCollectionDate,
                nameof(IBiomaterialCollection.CollectionDate)));
        }

        // Date range filtering for expiration date
        if (fromExpirationDate.HasValue || toExpirationDate.HasValue)
        {
            pipeline.Register(new DateRangeStep<IBiomaterialCollection>(
                fromExpirationDate,
                toExpirationDate,
                nameof(IBiomaterialCollection.ExpirationDate)));
        }

        // Laboratory filtering
        if (laboratoryId.HasValue)
        {
            pipeline.Register(new EqualityStep<IBiomaterialCollection, int?>(
                laboratoryId,
                $"{nameof(IBiomaterialCollection.InventoryInLaboratory)}.{nameof(IInventoryInLaboratory.LaboratoryId)}"));
        }

        // Inventory filtering
        if (inventoryId.HasValue) {
            pipeline.Register(new EqualityStep<IBiomaterialCollection, int?>(
                inventoryId,
                $"{nameof(IBiomaterialCollection.InventoryInLaboratory)}.{nameof(IInventoryInLaboratory.InventoryId)}"));
        }

        // Biomaterial filtering
        if (biomaterialId.HasValue)
        {
            pipeline.Register(new EqualityStep<IBiomaterialCollection, int?>(
                biomaterialId,
                $"{nameof(IBiomaterialCollection.Biomaterial)}.{nameof(IBiomaterial.BiomaterialId)}"));
        }

        // Execute the pipeline
        var filteredQuery = pipeline.Execute(query);

        // Apply pagination
        var pagedQuery = new PagingStep<IBiomaterialCollection>(pageNumber, pageSize)
            .Process(filteredQuery);

        return pagedQuery.ToList();
    }

    public async Task<IBiomaterialCollection?> UpdateAsync(IBiomaterialCollection entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}