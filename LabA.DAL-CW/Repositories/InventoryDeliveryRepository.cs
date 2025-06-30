using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class InventoryDeliveryRepository : IInventoryDeliveryRepository
{
    private readonly LabAContext _aContext;

    public InventoryDeliveryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IInventoryDelivery> CreateAsync(IInventoryDelivery entity)
    {
        var inventoryDelivery = (InventoryDelivery)entity;

        // Set the InventoryDeliveryId to 0 if it is not set
        if (inventoryDelivery.InventoryDeliveryId != 0)
        {
            inventoryDelivery.InventoryDeliveryId = 0;
        }

        // Set the InventoryInOrderId if InventoryInOrder is not null
        if (inventoryDelivery.InventoryInOrder != null)
        {
            inventoryDelivery.InventoryInOrderId = inventoryDelivery.InventoryInOrder.InventoryInOrderId;
        }

        if (inventoryDelivery.InventoryInLaboratory != null && inventoryDelivery.InventoryInLaboratory.Inventory != null)
        {
            inventoryDelivery.InventoryInLaboratory.InventoryId = inventoryDelivery.InventoryInLaboratory.Inventory.InventoryId;
            inventoryDelivery.InventoryInLaboratory.Inventory = null;
        }

        // Set the StatusId if Status is not null
        if (inventoryDelivery.Status != null)
        {
            inventoryDelivery.StatusId = inventoryDelivery.Status.StatusId;
        }

        // Set the Status to null if StatusId is not set
        inventoryDelivery.Status = null;

        // Set the InventoryInOrder to null if InventoryInOrderId is not set
        inventoryDelivery.InventoryInOrder = null;

        inventoryDelivery.CreateDatetime = DateTime.Now;
        inventoryDelivery.UpdateDatetime = null;

        await _aContext.InventoryDeliveries.AddAsync(inventoryDelivery);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryDelivery.InventoryDeliveryId))!;
    }

    public async Task<IInventoryDelivery?> ReadAsync(int id)
    {
        return await _aContext.InventoryDeliveries
            .Where(idl => idl.InventoryDeliveryId ==id)
            .Include(idl => idl.Status)
            .Include(idl => idl.InventoryInOrder).ThenInclude(io => io.Inventory)
            .Include(idl => idl.InventoryInOrder).ThenInclude(io => io.InventoryOrder)
            .Include(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .Include(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Laboratory).ThenInclude(l => l.City)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IInventoryDelivery>> ReadAllAsync()
    {
        return await _aContext.InventoryDeliveries
            .Include(idl => idl.Status)
            .Include(idl => idl.InventoryInOrder).ThenInclude(io => io.Inventory)
            .Include(idl => idl.InventoryInOrder).ThenInclude(io => io.InventoryOrder)
            .Include(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .Include(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Laboratory).ThenInclude(l => l.City)
            .AsNoTracking().ToListAsync();
    }

    public async Task<IInventoryDelivery?> UpdateAsync(IInventoryDelivery entity)
    {
        var inventoryDelivery = (InventoryDelivery)entity;
        var existingEntity = await _aContext.InventoryDeliveries
            .Where(idl => idl.InventoryDeliveryId == inventoryDelivery.InventoryDeliveryId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the InventoryInOrderId if InventoryInOrder is not null
        if (inventoryDelivery.InventoryInOrder != null)
        {
            inventoryDelivery.InventoryInOrderId = inventoryDelivery.InventoryInOrder.InventoryInOrderId;
        }

        // Set the InventoryInLaboratoryId if InventoryInLaboratory is not null
        if (inventoryDelivery.InventoryInLaboratory != null)
        {
            inventoryDelivery.InventoryInLaboratoryId = inventoryDelivery.InventoryInLaboratory.InventoryInLaboratoryId;
        }

        // Set the InventoryInLaboratory to null if InventoryInLaboratoryId is not set
        inventoryDelivery.InventoryInLaboratory = null;

        // Set the StatusId if Status is not null
        if (inventoryDelivery.Status != null)
        {
            inventoryDelivery.StatusId = inventoryDelivery.Status.StatusId;
        }
        // Set the Status to null if StatusId is not set
        inventoryDelivery.Status = null;

        // Prevent updating the CreateDatetime
        inventoryDelivery.CreateDatetime = existingEntity.CreateDatetime;

        // Prevent updating the UpdateDatetime
        inventoryDelivery.UpdateDatetime = existingEntity.UpdateDatetime;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(inventoryDelivery);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryDelivery.InventoryDeliveryId))!;
    }

    public async Task<IInventoryDelivery?> UpdateStatusAsync(int deliveryId, int status)
    {
        var entity = await _aContext.InventoryDeliveries
            .Include(idl => idl.InventoryInLaboratory)
            .FirstOrDefaultAsync(idl => idl.InventoryDeliveryId == deliveryId);
        if (entity == null) return null;

        var statusEntity = await _aContext.Statuses
            .FirstOrDefaultAsync(s => s.StatusId == status);
        if (statusEntity == null) return null;

        entity.StatusId = statusEntity.StatusId;

        entity.InventoryInLaboratory.Quantity = entity.StatusId switch
        {
            3 when entity is { InventoryInLaboratory: not null, Quantity: not null } => entity.Quantity.Value,
            4 when entity is { InventoryInLaboratory: not null, Quantity: not null } => 0,
            _ => entity.InventoryInLaboratory?.Quantity
        };

        entity.UpdateDatetime = DateTime.Now;

        _aContext.Entry(entity).State = EntityState.Modified;

        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(entity.InventoryDeliveryId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.InventoryDeliveries
            .Where(idl => idl.InventoryDeliveryId == id)
            .Include(idl => idl.Status)
            .Include(idl => idl.InventoryInOrder)
            .Include(idl => idl.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.InventoryDeliveries.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.InventoryDeliveries.RemoveRange(_aContext.InventoryDeliveries);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}