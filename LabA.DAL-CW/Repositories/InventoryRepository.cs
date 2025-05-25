using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly LabAContext _aContext;

    public InventoryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IInventory> CreateAsync(IInventory entity)
    {
        var inventory = (Inventory)entity;

        // Set the InventoryId to 0 if it is not set
        if (inventory.InventoryId != 0)
        {
            inventory.InventoryId = 0;
        }

        inventory.CreateDatetime = DateTime.Now;
        inventory.UpdateDatetime = null;

        await _aContext.Inventories.AddAsync(inventory);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventory.InventoryId))!;
    }

    public async Task<IInventory?> ReadAsync(int id)
    {
        return await _aContext.Inventories.FindAsync(id);
    }

    public async Task<IEnumerable<IInventory>> ReadAllAsync()
    {
        return await _aContext.Inventories.ToListAsync();
    }

    public async Task<IInventory?> UpdateAsync(IInventory entity)
    {
        var inventory = (Inventory)entity;
        var existingEntity = await _aContext.Inventories.FindAsync(inventory.InventoryId);
        if (existingEntity == null) return null;

        inventory.CreateDatetime = existingEntity.CreateDatetime;
        inventory.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(inventory);
        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(inventory.InventoryId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Inventories.FindAsync(id);
        if (entity == null) return false;
        _aContext.Inventories.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Inventories.RemoveRange(_aContext.Inventories);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}