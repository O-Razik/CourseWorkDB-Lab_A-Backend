using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class ClientOrderRepository : IClientOrderRepository
{
    private readonly LabAContext _aContext;

    public ClientOrderRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IClientOrder> CreateAsync(IClientOrder entity)
    {
        var clientOrder = new ClientOrder
        {
            Number = _aContext.ClientOrders.Count() + 1,
            StatusId = entity.StatusId,
            EmployeeId = entity.EmployeeId,
            ClientId = entity.ClientId,
            BiomaterialCollectionDate = entity.BiomaterialCollectionDate,
            Fullprice = entity.Fullprice,
            CreateDatetime = DateTime.Now
        };

        foreach (var analysisDto in entity.OrderAnalyses)
        {
            clientOrder.OrderAnalyses.Add(new OrderAnalysis
            {
                AnalysisId = analysisDto.AnalysisId
            });
        }

        foreach (var collectionDto in entity.BiomaterialCollections)
        {
            // Find the InventoryInLaboratory entity
            var inventory = await _aContext.InventoryInLaboratories
                .FirstOrDefaultAsync(i => i.InventoryInLaboratoryId == collectionDto.InventoryInLaboratoryId);

            if (inventory != null)
            {
                inventory.Quantity -= 1;

                // Optional: check for negative values
                if (inventory.Quantity < 0)
                    throw new InvalidOperationException("Not enough inventory available.");
            }

            clientOrder.BiomaterialCollections.Add(new BiomaterialCollection
            {
                BiomaterialId = collectionDto.BiomaterialId,
                InventoryInLaboratoryId = collectionDto.InventoryInLaboratoryId,
                CollectionDate = collectionDto.CollectionDate,
                ExpirationDate = collectionDto.ExpirationDate,
                Volume = collectionDto.Volume
            });
        }

        await _aContext.ClientOrders.AddAsync(clientOrder);
        await _aContext.SaveChangesAsync();

        return (await this.ReadAsync(clientOrder.ClientOrderId))!;
    }


    public async Task<IClientOrder?> ReadAsync(int id)
    {
        return await _aContext.ClientOrders
            .Where(cl => cl.ClientOrderId == id)
            .Include(co => co.Client).ThenInclude(cl => cl.Sex)
            .Include(co => co.Employee).ThenInclude(em => em.Position)
            .Include(co => co.Employee).ThenInclude(em => em.Laboratory).ThenInclude(l => l.City)
            .Include(co => co.OrderAnalyses).ThenInclude(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.Biomaterial)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(i => i.Inventory)
            .Include(co => co.Status)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IClientOrder>> ReadAllAsync()
    {
        return await _aContext.ClientOrders
            .Include(co => co.Client).ThenInclude(cl => cl.Sex)
            .Include(co => co.Employee).ThenInclude(em => em.Position)
            .Include(co => co.Employee).ThenInclude(em => em.Laboratory).ThenInclude(l => l.City)
            .Include(co => co.OrderAnalyses).ThenInclude(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.Biomaterial)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(i => i.Inventory)
            .Include(co => co.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<IClientOrder>> ReadAllByClientIdAsync(int clientId)
    {
        return await _aContext.ClientOrders
            .Where(cl => cl.ClientId == clientId)
            .Include(co => co.Client).ThenInclude(cl => cl.Sex)
            .Include(co => co.Employee).ThenInclude(em => em.Position)
            .Include(co => co.Employee).ThenInclude(em => em.Laboratory).ThenInclude(l => l.City)
            .Include(co => co.OrderAnalyses).ThenInclude(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.Biomaterial)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(i => i.Inventory)
            .Include(co => co.Status)
            .ToListAsync();
    }

    public async Task<IQueryable<IClientOrder>> QueryAsync()
    {
        var query = await _aContext.ClientOrders
            .Include(co => co.Client).ThenInclude(cl => cl.Sex)
            .Include(co => co.Employee).ThenInclude(em => em.Position)
            .Include(co => co.Employee).ThenInclude(em => em.Laboratory).ThenInclude(l => l.City)
            .Include(co => co.OrderAnalyses).ThenInclude(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.Biomaterial)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(i => i.Inventory)
            .Include(co => co.Status)
            .Select(co => (IClientOrder)co)
            .ToListAsync(); // Execute the query asynchronously

        return query.AsQueryable();
    }


    public async Task<IClientOrder?> UpdateAsync(IClientOrder entity)
    {
        var clientOrder = (ClientOrder)entity;
        var existingEntity = await _aContext.ClientOrders
            .Where(cl => cl.ClientOrderId == clientOrder.ClientOrderId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the ClientId if Client is not null
        if (clientOrder.Client != null)
        {
            clientOrder.ClientId = clientOrder.Client.ClientId;
        }
        // Set the Client to null if ClientId is not set
        clientOrder.Client = null;

        // Set the EmployeeId if Employee is not null
        if (clientOrder.Employee != null)
        {
            clientOrder.EmployeeId = clientOrder.Employee.EmployeeId;
        }
        // Set the Employee to null if EmployeeId is not set
        clientOrder.Employee = null;

        // Set the StatusId if Status is not null
        if (clientOrder.Status != null)
        {
            clientOrder.StatusId = clientOrder.Status.StatusId;
        }
        // Set the Status to null if StatusId is not set
        clientOrder.Status = null;

        // Prevent setting the CreateDatetime and UpdateDatetime to null
        clientOrder.CreateDatetime = existingEntity.CreateDatetime;
        clientOrder.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(clientOrder);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(clientOrder.ClientOrderId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.ClientOrders
            .Where(cl => cl.ClientOrderId == id)
            .Include(co => co.Client).ThenInclude(cl => cl.Sex)
            .Include(co => co.OrderAnalyses).ThenInclude(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.Biomaterial)
            .Include(co => co.BiomaterialCollections).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(i => i.Inventory)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.ClientOrders.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.ClientOrders.RemoveRange(_aContext.ClientOrders);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}