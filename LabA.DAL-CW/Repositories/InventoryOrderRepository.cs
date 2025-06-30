using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class InventoryOrderRepository : IInventoryOrderRepository
{
    private readonly LabAContext _aContext;

    public InventoryOrderRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IInventoryOrder> CreateAsync(IInventoryOrder entity)
    {
        var inventoryOrder = (InventoryOrder)entity;

        // Set the InventoryOrderId to 0 if it is not set
        if (inventoryOrder.InventoryOrderId != 0)
        {
            inventoryOrder.InventoryOrderId = 0;
        }

        // Set the SupplierId if Supplier is not null
        if (inventoryOrder.Supplier != null)
        {
            inventoryOrder.SupplierId = inventoryOrder.Supplier.SupplierId;
        }
        // Set the Supplier to null if SupplierId is not set
        inventoryOrder.Supplier = null;

        // Set the StatusId if Status is not null
        if (inventoryOrder.Status != null)
        {
            inventoryOrder.StatusId = inventoryOrder.Status.StatusId;
        }
        // Set the Status to null if StatusId is not set
        inventoryOrder.Status = null;

        // Set the InventoryInOrders to null if InventoryInOrders is not set
        if (inventoryOrder.InventoryInOrders != null)
        {
            foreach (var inventoryInOrder in inventoryOrder.InventoryInOrders)
            {
                inventoryInOrder.InventoryOrder = null;
                inventoryInOrder.Inventory = null;
            }
        }

        inventoryOrder.Number = _aContext.InventoryOrders.Count() + 1;

        inventoryOrder.CreateDatetime = DateTime.Now;
        inventoryOrder.UpdateDatetime = null;

        await _aContext.InventoryOrders.AddAsync(inventoryOrder);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryOrder.InventoryOrderId))!;
    }

    public async Task<IInventoryOrder?> ReadAsync(int id)
    {
        return await _aContext.InventoryOrders
            .Where(io => io.InventoryOrderId == id)
            .Include(io => io.Status)
            .Include(io => io.Supplier)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.Inventory)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.InventoryDeliveries).ThenInclude(idl => idl.Status)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.InventoryDeliveries).ThenInclude(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IInventoryOrder>> ReadAllAsync()
    {
        return await _aContext.InventoryOrders
            .Include(io => io.Status)
            .Include(io => io.Supplier)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.Inventory)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.InventoryDeliveries).ThenInclude(idl => idl.Status)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.InventoryDeliveries).ThenInclude(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .Include(io => io.InventoryInOrders).ThenInclude(iio => iio.InventoryDeliveries).ThenInclude(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Laboratory).ThenInclude(l => l.City)
            .ToListAsync();
    }


    public async Task<IInventoryOrder?> UpdateAsync(IInventoryOrder entity)
    {
        var inventoryOrder = (InventoryOrder)entity;
        var existingEntity = await _aContext.InventoryOrders
            .Where(io => io.InventoryOrderId == inventoryOrder.InventoryOrderId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the InventoryOrderId to 0 if it is not set
        if (inventoryOrder.InventoryOrderId != 0)
        {
            inventoryOrder.InventoryOrderId = 0;
        }

        // Set the SupplierId if Supplier is not null
        if (inventoryOrder.Supplier != null)
        {
            inventoryOrder.SupplierId = inventoryOrder.Supplier.SupplierId;
        }

        // Set the Supplier to null if SupplierId is not set
        inventoryOrder.Supplier = null;

        // Set the StatusId if Status is not null
        if (inventoryOrder.Status != null)
        {
            inventoryOrder.StatusId = inventoryOrder.Status.StatusId;
        }
        // Set the Status to null if StatusId is not set
        inventoryOrder.Status = null;

        // Set the InventoryInOrders to null if InventoryInOrders is not set
        if (inventoryOrder.InventoryInOrders != null)
        {
            foreach (var inventoryInOrder in inventoryOrder.InventoryInOrders)
            {
                inventoryInOrder.InventoryOrder = null;
                inventoryInOrder.Inventory = null;
            }
        }

        // Prevent updating the CreateDatetime
        inventoryOrder.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to now
        inventoryOrder.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(inventoryOrder);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryOrder.InventoryOrderId))!;
    }
    
    public async Task<IInventoryOrder?> CancelOrderAsync(int id)
    {
        var entity = await _aContext.InventoryOrders
            .Where(io => io.InventoryOrderId == id)
            .FirstOrDefaultAsync();
        if (entity == null) return null;

        // Set the Status to "Cancelled"
        entity.StatusId = _aContext.Statuses.FirstOrDefault(s => s.StatusName == "Cancelled")?.StatusId ?? 4;
        entity.UpdateDatetime = DateTime.Now;

        _aContext.Entry(entity).State = EntityState.Modified;
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(entity.InventoryOrderId))!;
    }

    public async Task<IInventoryOrder?> UpdateStatusAsync(int id)
    {
        var entity = await this.ReadAsync(id);
        if (entity == null) return null;

        // Check if all InventoryInOrder items are fully delivered
        var allCompleted = entity.InventoryInOrders.All(iio =>
        {
            var completedQty = iio.InventoryDeliveries
                .Where(d => d.Status is { StatusId: 3 })
                .Sum(d => d.Quantity ?? 0);

            return (iio.Quantity ?? 0) == completedQty;
        });

        var newStatusId = 1; // Новий

        if (allCompleted)
        {
            newStatusId = 3; // Завершений
        }
        else if (entity.InventoryInOrders.Any(iio => iio.InventoryDeliveries != null && iio.InventoryDeliveries.Any()))
        {
            newStatusId = 2; // В процесі
        }

        var dbEntity = await _aContext.InventoryOrders.FirstOrDefaultAsync(io => io.InventoryOrderId == id);
        if (dbEntity == null || dbEntity.StatusId == newStatusId) return await this.ReadAsync(id);
        dbEntity.StatusId = newStatusId;
        dbEntity.UpdateDatetime = DateTime.Now;
        _aContext.Entry(dbEntity).State = EntityState.Modified;
        await _aContext.SaveChangesAsync();

        return await this.ReadAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.InventoryOrders.FindAsync(id);
        if (entity == null) return false;
        _aContext.InventoryOrders.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.InventoryOrders.RemoveRange(_aContext.InventoryOrders);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}