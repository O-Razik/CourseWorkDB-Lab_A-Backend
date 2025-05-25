using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class AnalysisRepository : IAnalysisRepository
{
    private readonly LabAContext _aContext;

    public AnalysisRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IAnalysis> CreateAsync(IAnalysis entity)
    {
        var analysis = (Analysis)entity;
        analysis.CreateDatetime = DateTime.Now;
        analysis.UpdateDatetime = null;

        // Set the CategoryId if Category is not null
        if (analysis.Category != null)
        {
            analysis.CategoryId = analysis.Category.AnalysisCategoryId;
        }

        // Set the Category to null if CategoryId is not set
        analysis.Category = null;

        // Set the AnalysisBiomaterials to null if AnalysisBiomaterialId is not set
        foreach (var analysisBiomaterial in analysis.AnalysisBiomaterials)
        {
            if (analysisBiomaterial.Biomaterial != null)
            {
                analysisBiomaterial.BiomaterialId = analysisBiomaterial.Biomaterial.BiomaterialId;
            }
            // Set the Biomaterial to null if BiomaterialId is not set
            analysisBiomaterial.Biomaterial = null;

            // Set the Analysis to null if AnalysisId is not set
            analysisBiomaterial.AnalysisId = analysis.AnalysisId;
            analysisBiomaterial.Analysis = null;
        }

        // Set the AnalysisId to 0 if it is not set
        if (analysis.AnalysisId != 0)
        {
            analysis.AnalysisId = 0;
        }

        await _aContext.Analyses.AddAsync(analysis);
        await _aContext.SaveChangesAsync();
        return (await (this.ReadAsync(analysis.AnalysisId)!))!;
    }

    public async Task<IAnalysis?> ReadAsync(int id)
    {
        return await _aContext.Analyses
            .Where(a => a.AnalysisId == id)
            .Include(a => a.Category)
            .Include(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IAnalysis>> ReadAllAsync()
    {
        var result = await _aContext.Analyses
            .Include(a => a.Category)
            .Include(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .ToListAsync();
        return result;
    }

    public async Task<IAnalysis?> UpdateAsync(IAnalysis entity)
    {
        var analysis = (Analysis)entity;
        var existingEntity = await _aContext.Analyses
            .Where(a => a.AnalysisId == analysis.AnalysisId)
            .Include(a => a.Category)
            .Include(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Update the CategoryId if Category is not null
        if (analysis.Category != null)
        {
            existingEntity.CategoryId = analysis.Category.AnalysisCategoryId;
        }
        // Set the Category to null if CategoryId is not set
        existingEntity.Category = null;

        // Update the AnalysisBiomaterials
        foreach (var analysisBiomaterial in analysis.AnalysisBiomaterials)
        {
            var existingAnalysisBiomaterial = existingEntity.AnalysisBiomaterials
                .FirstOrDefault(ab => ab.AnalysisBiomaterialId == analysisBiomaterial.AnalysisBiomaterialId);
            if (existingAnalysisBiomaterial != null)
            {
                existingAnalysisBiomaterial.BiomaterialId = analysisBiomaterial.BiomaterialId;
                existingAnalysisBiomaterial.AnalysisId = analysis.AnalysisId;
            }
        }
        // Set the AnalysisBiomaterials to null if AnalysisBiomaterialId is not set
        foreach (var analysisBiomaterial in existingEntity.AnalysisBiomaterials)
        {
            if (analysisBiomaterial.Biomaterial != null)
            {
                analysisBiomaterial.BiomaterialId = analysisBiomaterial.Biomaterial.BiomaterialId;
            }
            // Set the Biomaterial to null if BiomaterialId is not set
            analysisBiomaterial.Biomaterial = null;
            // Set the Analysis to null if AnalysisId is not set
            analysisBiomaterial.AnalysisId = analysis.AnalysisId;
            analysisBiomaterial.Analysis = null;
        }

        // Preserve the original CreateDatetime
        analysis.CreateDatetime = existingEntity.CreateDatetime;

        // Set the UpdateDatetime to the current time
        analysis.UpdateDatetime = DateTime.Now;

        // Update the existing entity with the new values
        // Set the AnalysisId if Analysis is not null
        if (analysis.AnalysisId != 0)
        {
            existingEntity.AnalysisId = analysis.AnalysisId;
        }

        _aContext.Entry(existingEntity).CurrentValues.SetValues(analysis);
        await _aContext.SaveChangesAsync();
        return (await (this.ReadAsync(analysis.AnalysisId)!))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Analyses
            .Where(a => a.AnalysisId == id)
            .Include(a => a.Category)
            .Include(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.Analyses.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Analyses.RemoveRange(_aContext.Analyses);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}