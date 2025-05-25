using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class InventoryInOrderRepository : IInventoryInOrderRepository
{
    private readonly LabAContext _aContext;

    public InventoryInOrderRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IInventoryInOrder> CreateAsync(IInventoryInOrder entity)
    {
        var inventoryInOrder = (InventoryInOrder)entity;

        if (inventoryInOrder.InventoryOrder != null)
        {
            inventoryInOrder.InventoryOrderId = inventoryInOrder.InventoryOrder.InventoryOrderId;
        }
        inventoryInOrder.InventoryOrder = null;

        // Set the InventoryId if Inventory is not null
        if (inventoryInOrder.Inventory != null)
        {
            inventoryInOrder.InventoryId = inventoryInOrder.Inventory.InventoryId;
        }
        // Set the Inventory to null if InventoryId is not set
        inventoryInOrder.Inventory = null;

        inventoryInOrder.CreateDatetime = DateTime.Now;
        inventoryInOrder.UpdateDatetime = null;

        await _aContext.InventoryInOrders.AddAsync(inventoryInOrder);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryInOrder.InventoryInOrderId))!;
    }

    public async Task<IInventoryInOrder?> ReadAsync(int id)
    {
        return await _aContext.InventoryInOrders
            .Where(iio => iio.InventoryInOrderId == id)
            .Include(io => io.Inventory)
            .Include(io => io.InventoryDeliveries).ThenInclude(io => io.Status)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IInventoryInOrder>> ReadAllAsync()
    {
        return await _aContext.InventoryInOrders
            .Include(io => io.Inventory)
            .Include(io => io.InventoryDeliveries).ThenInclude(io => io.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<IInventoryInOrder>> ReadAllByInventoryOrderIdAsync(int inventoryOrderId)
    {
        return await _aContext.InventoryInOrders
            .Where(iio => iio.InventoryOrderId == inventoryOrderId)
            .Include(io => io.Inventory)
            .ToListAsync();
    }

    public async Task<IInventoryInOrder?> UpdateAsync(IInventoryInOrder entity)
    {
        var inventoryInOrder = (InventoryInOrder)entity;
        var existingEntity = await _aContext.InventoryInOrders
            .Where(iio => iio.InventoryInOrderId == inventoryInOrder.InventoryInOrderId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the InventoryId if Inventory is not null
        if (inventoryInOrder.Inventory != null)
        {
            inventoryInOrder.InventoryId = inventoryInOrder.Inventory.InventoryId;
        }
        // Set the Inventory to null if InventoryId is not set
        inventoryInOrder.Inventory = null;

        // Set the InventoryOrderId if InventoryOrder is not null
        if (inventoryInOrder.InventoryOrder != null)
        {
            inventoryInOrder.InventoryOrderId = inventoryInOrder.InventoryOrder.InventoryOrderId;
        }

        // Set the InventoryOrder to null if InventoryOrderId is not set
        inventoryInOrder.InventoryOrder = null;

        // Prevent setting the CreateDatetime and UpdateDatetime
        inventoryInOrder.CreateDatetime = existingEntity.CreateDatetime;
        inventoryInOrder.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(inventoryInOrder);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryInOrder.InventoryInOrderId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.InventoryInOrders
            .Where(iio => iio.InventoryInOrderId == id)
            .Include(io => io.Inventory)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.InventoryInOrders.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.InventoryInOrders.RemoveRange(_aContext.InventoryInOrders);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}