using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class AnalysisCenterRepository : IAnalysisCenterRepository
{
    private readonly LabAContext _aContext;

    public AnalysisCenterRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IAnalysisCenter> CreateAsync(IAnalysisCenter entity)
    {
        var analysisCenter = (AnalysisCenter)entity;
        analysisCenter.CreateDatetime = DateTime.Now;
        analysisCenter.UpdateDatetime = null;

        // Set the CityId if City is not null
        if (analysisCenter.City != null)
        {
            analysisCenter.CityId = analysisCenter.City.CityId;
        }
        // Set the City to null if CityId is not set
        analysisCenter.City = null;

        await _aContext.AnalysisCenters.AddAsync(analysisCenter);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisCenter.AnalysisCenterId))!;
    }

    public async Task<IAnalysisCenter?> ReadAsync(int id)
    {
        var result = await _aContext.AnalysisCenters
            .Where(ac => ac.AnalysisCenterId == id)
            .Include(ac => ac.City)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<IAnalysisCenter>> ReadAllAsync()
    {
        var result = await _aContext.AnalysisCenters
            .Include(ac => ac.City)
            .ToListAsync();
        return result;
    }

    public async Task<IAnalysisCenter?> UpdateAsync(IAnalysisCenter entity)
    {
        var analysisCenter = (AnalysisCenter)entity;
        var existingEntity = await _aContext.AnalysisCenters
            .Where(ac => ac.AnalysisCenterId == analysisCenter.AnalysisCenterId)
            .FirstOrDefaultAsync();

        if (existingEntity == null) return null;

        // Preserve the original CreateDatetime
        analysisCenter.CreateDatetime = existingEntity.CreateDatetime;

        // Set update time to now
        analysisCenter.UpdateDatetime = DateTime.Now;

        // Update all values except collections and navigation properties
        _aContext.Entry(existingEntity).CurrentValues.SetValues(analysisCenter);

        await _aContext.SaveChangesAsync();
        return existingEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.AnalysisCenters
            .Where(ac => ac.AnalysisCenterId == id)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.AnalysisCenters.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.AnalysisCenters.RemoveRange(_aContext.AnalysisCenters);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IAnalysisCenter?> GetByCity(int cityId)
    {
        var result = await _aContext.AnalysisCenters
            .Include(x => x.City)
            .FirstOrDefaultAsync(x => x.CityId == cityId);
        return result;
    }
}