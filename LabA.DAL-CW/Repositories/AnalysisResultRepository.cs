using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class AnalysisResultRepository : IAnalysisResultRepository
{
    private readonly LabAContext _aContext;

    public AnalysisResultRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IAnalysisResult> CreateAsync(IAnalysisResult entity)
    {
        var analysisResult = (AnalysisResult)entity;
        analysisResult.CreateDatetime = DateTime.Now;
        analysisResult.UpdateDatetime = null;

        // Set the OrderAnalysisId if OrderAnalysis is not null
        if (analysisResult.OrderAnalysis != null)
        {
            analysisResult.OrderAnalysisId = analysisResult.OrderAnalysis.OrderAnalysisId;
        }
        // Set the OrderAnalysis to null if OrderAnalysisId is not set
        analysisResult.OrderAnalysis = null;

        // Set the AnalysisCenterId if AnalysisCenter is not null
        if (analysisResult.AnalysisCenter != null)
        {
            analysisResult.AnalysisCenterId = analysisResult.AnalysisCenter.AnalysisCenterId;
        }
        // Set the AnalysisCenter to null if AnalysisCenterId is not set
        analysisResult.AnalysisCenter = null;

        // Set the AnalysisResultId to 0 if it is not set
        if (analysisResult.AnalysisResultId != 0)
        {
            analysisResult.AnalysisResultId = 0;
        }
        await _aContext.AnalysisResults.AddAsync(analysisResult);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisResult.AnalysisResultId))!;
    }

    public async Task<IAnalysisResult?> ReadAsync(int id)
    {
        return await _aContext.AnalysisResults.Where(ar => ar.AnalysisResultId == id)
            .Include(ar => ar.AnalysisCenter).ThenInclude(ac => ac.City)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.Analysis)
                .ThenInclude(a => a.Category)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Status)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Client).ThenInclude(c => c.Sex)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Employee).ThenInclude(e => e.Position)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Employee).ThenInclude(e => e.Laboratory)
                .ThenInclude(l => l.City)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IAnalysisResult>> ReadAllAsync()
    {
        return await _aContext.AnalysisResults
            .Include(ar => ar.AnalysisCenter).ThenInclude(ac => ac.City)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.Analysis)
                .ThenInclude(a => a.Category)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Status)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Client).ThenInclude(c => c.Sex)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Employee).ThenInclude(e => e.Position)
            .Include(ar => ar.OrderAnalysis).ThenInclude(oa => oa.ClientOrder)
                .ThenInclude(co => co.Employee).ThenInclude(e => e.Laboratory)
                .ThenInclude(l => l.City)
            .ToListAsync();
    }

    public async Task<IAnalysisResult?> UpdateAsync(IAnalysisResult entity)
    {
        var analysisResult = (AnalysisResult)entity;
        var existingEntity = await _aContext.AnalysisResults
            .Where(ar => ar.AnalysisResultId == analysisResult.AnalysisResultId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the OrderAnalysisId if OrderAnalysis is not null
        if (analysisResult.OrderAnalysis != null)
        {
            analysisResult.OrderAnalysisId = analysisResult.OrderAnalysis.OrderAnalysisId;
        }

        // Set the OrderAnalysis to null if OrderAnalysisId is not set
        analysisResult.OrderAnalysis = null;

        // Set the AnalysisCenterId if AnalysisCenter is not null
        if (analysisResult.AnalysisCenter != null)
        {
            analysisResult.AnalysisCenterId = analysisResult.AnalysisCenter.AnalysisCenterId;
        }

        // Set the AnalysisCenter to null if AnalysisCenterId is not set
        analysisResult.AnalysisCenter = null;

        // Preserve the existing values for CreateDatetime and UpdateDatetime
        analysisResult.CreateDatetime = existingEntity.CreateDatetime;
        analysisResult.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(analysisResult);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(analysisResult.AnalysisResultId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.AnalysisResults.Where(ar => ar.AnalysisResultId == id)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.AnalysisResults.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.AnalysisResults.RemoveRange(_aContext.AnalysisResults);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}