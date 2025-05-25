using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class BiomaterialDeliveryRepository : IBiomaterialDeliveryRepository
{
    private readonly LabAContext _aContext;

    public BiomaterialDeliveryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IBiomaterialDelivery> CreateAsync(IBiomaterialDelivery entity)
    {
        var biomaterialDelivery = (BiomaterialDelivery)entity;

        biomaterialDelivery.CreateDatetime = DateTime.Now;
        biomaterialDelivery.UpdateDatetime = null;

        // Set the BiomaterialCollectionId if BiomaterialCollection is not null
        if (biomaterialDelivery.BiomaterialCollection != null)
        {
            biomaterialDelivery.BiomaterialCollectionId = biomaterialDelivery.BiomaterialCollection.BiomaterialCollectionId;
        }
        // Set the BiomaterialCollection to null if BiomaterialCollectionId is not set
        biomaterialDelivery.BiomaterialCollection = null;

        // Set the AnalysisCenterId if AnalysisCenter is not null
        if (biomaterialDelivery.AnalysisCenter != null)
        {
            biomaterialDelivery.AnalysisCenterId = biomaterialDelivery.AnalysisCenter.AnalysisCenterId;
        }
        // Set the AnalysisCenter to null if AnalysisCenterId is not set
        biomaterialDelivery.AnalysisCenter = null;

        // Set the StatusId if Status is not null
        if (biomaterialDelivery.Status != null)
        {
            biomaterialDelivery.StatusId = biomaterialDelivery.Status.StatusId;
        }

        // Set the Status to null if StatusId is not set
        biomaterialDelivery.Status = null;

        // Set the BiomaterialDeliveryId to 0 if it is not set
        if (biomaterialDelivery.BiomaterialDeliveryId != 0)
        {
            biomaterialDelivery.BiomaterialDeliveryId = 0;
        }

        await _aContext.BiomaterialDeliveries.AddAsync(biomaterialDelivery);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(biomaterialDelivery.BiomaterialDeliveryId))!;
    }

    public async Task<IBiomaterialDelivery?> ReadAsync(int id)
    {
        return await _aContext.BiomaterialDeliveries
            .Where(bc => bc.BiomaterialDeliveryId == id)
            .Include(bd => bd.Status)
            .Include(bc => bc.BiomaterialCollection).ThenInclude(bc => bc.Biomaterial)
            .Include(bc => bc.BiomaterialCollection).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(il => il.Inventory)
            .Include(bc => bc.AnalysisCenter).ThenInclude(ac => ac.City)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IBiomaterialDelivery>> ReadAllAsync()
    {
        return await _aContext.BiomaterialDeliveries
            .Include(bd => bd.Status)
            .Include(bc => bc.BiomaterialCollection).ThenInclude(bc => bc.Biomaterial)
            .Include(bc => bc.BiomaterialCollection).ThenInclude(bc => bc.InventoryInLaboratory).ThenInclude(il => il.Inventory)
            .Include(bc => bc.AnalysisCenter).ThenInclude(ac => ac.City)
            .ToListAsync();
    }

    public async Task<IBiomaterialDelivery?> UpdateAsync(IBiomaterialDelivery entity)
    {
        var biomaterialDelivery = (BiomaterialDelivery)entity;
        var existingEntity = await _aContext.BiomaterialDeliveries
            .Where(bc => bc.BiomaterialDeliveryId == biomaterialDelivery.BiomaterialDeliveryId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the BiomaterialCollectionId if BiomaterialCollection is not null
        if (biomaterialDelivery.BiomaterialCollection != null)
        {
            biomaterialDelivery.BiomaterialCollectionId = biomaterialDelivery.BiomaterialCollection.BiomaterialCollectionId;
        }

        // Set the BiomaterialCollection to null if BiomaterialCollectionId is not set
        biomaterialDelivery.BiomaterialCollection = null;

        // Set the AnalysisCenterId if AnalysisCenter is not null
        if (biomaterialDelivery.AnalysisCenter != null)
        {
            biomaterialDelivery.AnalysisCenterId = biomaterialDelivery.AnalysisCenter.AnalysisCenterId;
        }

        // Set the AnalysisCenter to null if AnalysisCenterId is not set
        biomaterialDelivery.AnalysisCenter = null;

        // Set the StatusId if Status is not null
        if (biomaterialDelivery.Status != null)
        {
            biomaterialDelivery.StatusId = biomaterialDelivery.Status.StatusId;
        }
        // Set the Status to null if StatusId is not set
        biomaterialDelivery.Status = null;

        // Prevent updating the CreateDatetime
        biomaterialDelivery.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to the current time
        biomaterialDelivery.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(biomaterialDelivery);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(biomaterialDelivery.BiomaterialDeliveryId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.BiomaterialDeliveries
            .Where(bc => bc.BiomaterialDeliveryId == id)
            .Include(bd => bd.Status)
            .Include(bc => bc.BiomaterialCollection).ThenInclude(bc => bc.Biomaterial)
            .Include(bc => bc.AnalysisCenter).ThenInclude(ac => ac.City)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.BiomaterialDeliveries.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.BiomaterialDeliveries.RemoveRange(_aContext.BiomaterialDeliveries);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}