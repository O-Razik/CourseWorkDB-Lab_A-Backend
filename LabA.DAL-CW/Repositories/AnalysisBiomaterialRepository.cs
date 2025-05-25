using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class AnalysisBiomaterialRepository : IAnalysisBiomaterialRepository
{
    private readonly LabAContext _aContext;

    public AnalysisBiomaterialRepository(LabAContext aContext)
    {
        this._aContext = aContext;
    }
    

    public async Task<IAnalysisBiomaterial> CreateAsync(IAnalysisBiomaterial entity)
    {
        var analysisBiomaterial = (AnalysisBiomaterial)entity;
        analysisBiomaterial.CreateDatetime = DateTime.Now;
        analysisBiomaterial.UpdateDatetime = null;

        // Set the BiomaterialId if Biomaterial is not null
        if (analysisBiomaterial.Biomaterial != null)
        {
            analysisBiomaterial.BiomaterialId = analysisBiomaterial.Biomaterial.BiomaterialId;
        }
        // Set the Biomaterial to null if BiomaterialId is not set
        analysisBiomaterial.Biomaterial = null;

        // Set the AnalysisId if Analysis is not null
        if (analysisBiomaterial.Analysis != null)
        {
            analysisBiomaterial.AnalysisId = analysisBiomaterial.Analysis.AnalysisId;
        }

        // Set the Analysis to null if AnalysisId is not set
        analysisBiomaterial.Analysis = null;

        // Set the AnalysisBiomaterialId to 0 if it is not set
        if (analysisBiomaterial.AnalysisBiomaterialId != 0)
        {
            analysisBiomaterial.AnalysisBiomaterialId = 0;
        }

        this._aContext.AnalysisBiomaterials.Add(analysisBiomaterial);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisBiomaterial.AnalysisBiomaterialId))!;
    }

    public async Task<IAnalysisBiomaterial?> ReadAsync(int id)
    {
        var result = await this._aContext.AnalysisBiomaterials
            .Where(ab => ab.AnalysisBiomaterialId == id)
            .Include(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<IEnumerable<IAnalysisBiomaterial>> ReadAllAsync()
    {
        return await this._aContext.AnalysisBiomaterials
            .Include(ab => ab.Biomaterial)
            .ToListAsync();
    }

    public async Task<IEnumerable<IAnalysisBiomaterial>> ReadAllByAnalysisIdAsync(int analysisId)
    {
        return await this._aContext.AnalysisBiomaterials
            .Where(ab => ab.AnalysisId == analysisId)
            .Include(ab => ab.Biomaterial)
            .ToListAsync();
    }

    public async Task<IAnalysisBiomaterial?> UpdateAsync(IAnalysisBiomaterial entity)
    {
        try
        {
            var analysisBiomaterial = (AnalysisBiomaterial)entity;
            var existingEntity = await this._aContext.AnalysisBiomaterials
                .FirstOrDefaultAsync(ab => ab.AnalysisBiomaterialId == analysisBiomaterial.AnalysisBiomaterialId);

            if (existingEntity == null)
            {
                return null;
            }

            // Preserve the original CreateDatetime
            analysisBiomaterial.CreateDatetime = existingEntity.CreateDatetime;

            // Set the UpdateDatetime to the current time
            analysisBiomaterial.UpdateDatetime = DateTime.Now;

            // Set the BiomaterialId if Biomaterial is not null
            if (analysisBiomaterial.Biomaterial != null)
            {
                analysisBiomaterial.BiomaterialId = analysisBiomaterial.Biomaterial.BiomaterialId;
            }

            // Set the Biomaterial to null if BiomaterialId is not set
            analysisBiomaterial.Biomaterial = null;

            // Set the AnalysisId if Analysis is not null
            if (analysisBiomaterial.Analysis != null)
            {
                analysisBiomaterial.AnalysisId = analysisBiomaterial.Analysis.AnalysisId;
            }

            // Set the Analysis to null if AnalysisId is not set
            analysisBiomaterial.Analysis = null;

            // Update the existing entity with the new values
            this._aContext.Entry(existingEntity).CurrentValues.SetValues(analysisBiomaterial);
            await _aContext.SaveChangesAsync();
            return await (this.ReadAsync(analysisBiomaterial.AnalysisBiomaterialId));
        }
        catch (Exception)
        {
            return null;
        }
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await this._aContext.AnalysisBiomaterials
            .Where(ab => ab.AnalysisBiomaterialId == id)
            .Include(ab => ab.Biomaterial)
            .FirstOrDefaultAsync(); ;
        if (entity == null)
        {
            return false;
        }

        this._aContext.AnalysisBiomaterials.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            this._aContext.AnalysisBiomaterials.RemoveRange(this._aContext.AnalysisBiomaterials);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}