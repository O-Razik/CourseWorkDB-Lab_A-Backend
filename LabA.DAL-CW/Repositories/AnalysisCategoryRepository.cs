using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class AnalysisCategoryRepository : IAnalysisCategoryRepository
{
    private readonly LabAContext _aContext;

    public AnalysisCategoryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IAnalysisCategory> CreateAsync(IAnalysisCategory entity)
    {
        var analysisCategory = (AnalysisCategory)entity;
        analysisCategory.CreateDatetime = DateTime.Now;
        analysisCategory.UpdateDatetime = null;

        // Set the AnalysisId to 0 if it is not set
        if (analysisCategory.AnalysisCategoryId != 0)
        {
            analysisCategory.AnalysisCategoryId = 0;
        }

        await _aContext.AnalysisCategories.AddAsync(analysisCategory);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisCategory.AnalysisCategoryId))!;
    }

    public async Task<IAnalysisCategory?> ReadAsync(int id)
    {
        return await _aContext.AnalysisCategories.FindAsync(id);
    }

    public async Task<IEnumerable<IAnalysisCategory>> ReadAllAsync()
    {
        return await _aContext.AnalysisCategories.ToListAsync();
    }

    public async Task<IAnalysisCategory?> UpdateAsync(IAnalysisCategory entity)
    {
        var analysisCategory = (AnalysisCategory)entity;
        var existingEntity = await _aContext.AnalysisCategories.FindAsync(analysisCategory.AnalysisCategoryId);
        if (existingEntity == null) return null;

        // Prevent updating the CreateDatetime
        analysisCategory.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to the current time
        analysisCategory.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(analysisCategory);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisCategory.AnalysisCategoryId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.AnalysisCategories.FindAsync(id);
        if (entity == null) return false;
        _aContext.AnalysisCategories.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.AnalysisCategories.RemoveRange(_aContext.AnalysisCategories);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IAnalysisCategory?> GetByName(string? itemCategory)
    {
        var category = await _aContext.AnalysisCategories.FirstOrDefaultAsync(c => c.Category == itemCategory);
        return category;
    }
}