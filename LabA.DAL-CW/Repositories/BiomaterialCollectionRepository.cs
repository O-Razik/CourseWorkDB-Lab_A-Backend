using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class BiomaterialCollectionRepository : IBiomaterialCollectionRepository
{
    private readonly LabAContext _aContext;

    public BiomaterialCollectionRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IBiomaterialCollection> CreateAsync(IBiomaterialCollection entity)
    {
        var biomaterialCollection = (BiomaterialCollection)entity;
        biomaterialCollection.CreateDatetime = DateTime.Now;
        biomaterialCollection.UpdateDatetime = null;
        biomaterialCollection.BiomaterialCollectionId = 0;

        // Set the BiomaterialId if Biomaterial is not null
        if (biomaterialCollection.Biomaterial != null)
        {
            biomaterialCollection.BiomaterialId = biomaterialCollection.Biomaterial.BiomaterialId;
        }

        // Set the Biomaterial to null if BiomaterialId is not set
        biomaterialCollection.Biomaterial = null;
        // Set the InventoryInLaboratoryId if InventoryInLaboratory is not null
        if (biomaterialCollection.InventoryInLaboratory != null)
        {
            biomaterialCollection.InventoryInLaboratoryId = biomaterialCollection.InventoryInLaboratory.InventoryInLaboratoryId;
        }

        // Set the InventoryInLaboratory to null if InventoryInLaboratoryId is not set
        biomaterialCollection.InventoryInLaboratory = null;

        // Set the BiomaterialCollectionId to 0 if it is not set
        if (biomaterialCollection.BiomaterialCollectionId != 0)
        {
            biomaterialCollection.BiomaterialCollectionId = 0;
        }

        await _aContext.BiomaterialCollections.AddAsync(biomaterialCollection);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(biomaterialCollection.BiomaterialCollectionId))!;
    }

    public async Task<IBiomaterialCollection?> ReadAsync(int id)
    {
        return await _aContext.BiomaterialCollections
            .Where(bc => bc.BiomaterialCollectionId == id)
            .Include(bc => bc.Biomaterial)
            .Include(bc => bc.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .Include(bc => bc.ClientOrder)
            .Include(bc => bc.BiomaterialDeliveries)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IBiomaterialCollection>> ReadAllAsync()
    {
        return await _aContext.BiomaterialCollections
            .Include(bc => bc.Biomaterial)
            .Include(bc => bc.InventoryInLaboratory).ThenInclude(iil => iil.Inventory)
            .Include(bc => bc.ClientOrder)
            .Include(bc => bc.BiomaterialDeliveries)
            .ToListAsync();
    }

    public async Task<IBiomaterialCollection?> UpdateAsync(IBiomaterialCollection entity)
    {
        var biomaterialCollection = (BiomaterialCollection)entity;
        var existingEntity = await _aContext.BiomaterialCollections
            .Where(bc => bc.BiomaterialCollectionId == biomaterialCollection.BiomaterialCollectionId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the BiomaterialId if Biomaterial is not null
        if (biomaterialCollection.Biomaterial != null)
        {
            biomaterialCollection.BiomaterialId = biomaterialCollection.Biomaterial.BiomaterialId;
        }

        // Set the Biomaterial to null if BiomaterialId is not set
        biomaterialCollection.Biomaterial = null;

        // Set the InventoryInLaboratoryId if InventoryInLaboratory is not null
        if (biomaterialCollection.InventoryInLaboratory != null)
        {
            biomaterialCollection.InventoryInLaboratoryId = biomaterialCollection.InventoryInLaboratory.InventoryInLaboratoryId;
        }

        // Set the InventoryInLaboratory to null if InventoryInLaboratoryId is not set
        biomaterialCollection.InventoryInLaboratory = null;

        // Prevent updating the CreateDatetime and UpdateDatetime properties
        biomaterialCollection.CreateDatetime = existingEntity.CreateDatetime;
        biomaterialCollection.UpdateDatetime = DateTime.Now;

        // Update all values except collections and navigation properties
        _aContext.Entry(existingEntity).CurrentValues.SetValues(biomaterialCollection);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(biomaterialCollection.BiomaterialCollectionId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.BiomaterialCollections
            .Where(bc => bc.BiomaterialCollectionId == id)
            .Include(bc => bc.Biomaterial)
            .Include(bc => bc.InventoryInLaboratory)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.BiomaterialCollections.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.BiomaterialCollections.RemoveRange(_aContext.BiomaterialCollections);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}