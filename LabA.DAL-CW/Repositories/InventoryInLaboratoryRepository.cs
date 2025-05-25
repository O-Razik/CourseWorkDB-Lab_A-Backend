using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class InventoryInLaboratoryRepository : IInventoryInLaboratoryRepository
{
    private readonly LabAContext _aContext;

    public InventoryInLaboratoryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IInventoryInLaboratory> CreateAsync(IInventoryInLaboratory entity)
    {
        var inventoryInLaboratory = (InventoryInLaboratory)entity;
        // Set the InventoryInLaboratoryId to 0 if it is not set
        if (inventoryInLaboratory.InventoryInLaboratoryId != 0)
        {
            inventoryInLaboratory.InventoryInLaboratoryId = 0;
        }

        // Set the LaboratoryId if Laboratory is not null
        if (inventoryInLaboratory.Laboratory != null)
        {
            inventoryInLaboratory.LaboratoryId = inventoryInLaboratory.Laboratory.LaboratoryId;
        }

        // Set the Laboratory to null if LaboratoryId is not set
        inventoryInLaboratory.Laboratory = null;

        // Set the InventoryId if Inventory is not null
        if (inventoryInLaboratory.Inventory != null)
        {
            inventoryInLaboratory.InventoryId = inventoryInLaboratory.Inventory.InventoryId;
        }

        // Set the Inventory to null if InventoryId is not set
        inventoryInLaboratory.Inventory = null;

        // Set the InventoryInLaboratoryId to 0 if it is not set
        if (inventoryInLaboratory.InventoryInLaboratoryId != 0)
        {
            inventoryInLaboratory.InventoryInLaboratoryId = 0;
        }

        // Set the CreateDatetime to now
        inventoryInLaboratory.CreateDatetime = DateTime.Now;
        // Set the UpdateDatetime to null
        inventoryInLaboratory.UpdateDatetime = null;

        await _aContext.InventoryInLaboratories.AddAsync(inventoryInLaboratory);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(inventoryInLaboratory.InventoryInLaboratoryId))!;
    }

    public async Task<IInventoryInLaboratory?> ReadAsync(int id)
    {
        return await _aContext.InventoryInLaboratories
            .Where(iil => iil.InventoryInLaboratoryId == id)
            .Include(iil => iil.Laboratory).ThenInclude(l => l.City)
            .Include(iil => iil.Inventory)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IInventoryInLaboratory>> ReadAllAsync()
    {
        return await _aContext.InventoryInLaboratories
            .Include(iil => iil.Laboratory).ThenInclude(l => l.City)
            .Include(iil => iil.Inventory)
            .ToListAsync();
    }

    public async Task<IInventoryInLaboratory?> UpdateAsync(IInventoryInLaboratory entity)
    {
        var inventoryInLaboratory = (InventoryInLaboratory)entity;
        var existingEntity = await _aContext.InventoryInLaboratories
            .Where(iil => iil.InventoryInLaboratoryId == inventoryInLaboratory.InventoryInLaboratoryId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Update properties
        if (inventoryInLaboratory.Laboratory != null)
            existingEntity.LaboratoryId = inventoryInLaboratory.Laboratory.LaboratoryId;
        if (inventoryInLaboratory.Inventory != null)
            existingEntity.InventoryId = inventoryInLaboratory.Inventory.InventoryId;
        existingEntity.ExpirationDate = inventoryInLaboratory.ExpirationDate;
        existingEntity.UpdateDatetime = DateTime.Now;

        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(existingEntity.InventoryInLaboratoryId);
    }



    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.InventoryInLaboratories
            .Where(iil => iil.InventoryInLaboratoryId == id)
            .Include(iil => iil.Laboratory).ThenInclude(l => l.City)
            .Include(iil => iil.Inventory)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.InventoryInLaboratories.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.InventoryInLaboratories.RemoveRange(_aContext.InventoryInLaboratories);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<IInventoryInLaboratory>> GetByLaboratoryAsync(int laboratoryId)
    {
        return await _aContext.InventoryInLaboratories
            .Where(i => i.LaboratoryId == laboratoryId)
            .Include(iil => iil.Laboratory).ThenInclude(l => l.City)
            .Include(iil => iil.Inventory)
            .ToListAsync();
    }
}