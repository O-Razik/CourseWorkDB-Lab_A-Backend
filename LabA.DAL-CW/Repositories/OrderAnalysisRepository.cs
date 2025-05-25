using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class OrderAnalysisRepository : IOrderAnalysisRepository
{
    private readonly LabAContext _aContext;

    public OrderAnalysisRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IOrderAnalysis> CreateAsync(IOrderAnalysis entity)
    {
        var orderAnalysis = (OrderAnalysis)entity;

        // Set the OrderAnalysisId to 0 if it is not set
        if (orderAnalysis.OrderAnalysisId != 0)
        {
            orderAnalysis.OrderAnalysisId = 0;
        }

        // Set the ClientOrderId if ClientOrder is not null
        if (orderAnalysis.ClientOrder != null)
        {
            orderAnalysis.ClientOrderId = orderAnalysis.ClientOrder.ClientOrderId;
        }

        // Set the ClientOrder to null if ClientOrderId is not set
        orderAnalysis.ClientOrder = null;

        // Set the AnalysisId if Analysis is not null
        if (orderAnalysis.Analysis != null)
        {
            orderAnalysis.AnalysisId = orderAnalysis.Analysis.AnalysisId;
        }

        // Set the Analysis to null if AnalysisId is not set
        orderAnalysis.Analysis = null;

        orderAnalysis.CreateDatetime = DateTime.Now;
        orderAnalysis.UpdateDatetime = null;

        await _aContext.OrderAnalyses.AddAsync(orderAnalysis);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(orderAnalysis.OrderAnalysisId))!;
    }

    public async Task<IOrderAnalysis?> ReadAsync(int id)
    {
        return await _aContext.OrderAnalyses
            .Where(oa => oa.OrderAnalysisId == id)
            .Include(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(oa => oa.Analysis).ThenInclude(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IOrderAnalysis>> ReadAllAsync()
    {
        return await _aContext.OrderAnalyses
            .Include(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(oa => oa.Analysis).ThenInclude(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .ToListAsync();
    }

    public async Task<IEnumerable<IOrderAnalysis>> ReadAllByClientOrderAsync(int clientOrderId)
    {
        return await _aContext.OrderAnalyses
            .Where(oa => oa.ClientOrderId == clientOrderId)
            .Include(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(oa => oa.Analysis).ThenInclude(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .ToListAsync();
    }

    public async Task<IOrderAnalysis?> UpdateAsync(IOrderAnalysis entity)
    {
        var orderAnalysis = (OrderAnalysis)entity;
        var existingEntity = await _aContext.OrderAnalyses
            .Where(oa => oa.OrderAnalysisId == orderAnalysis.OrderAnalysisId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the ClientOrderId if ClientOrder is not null
        if (orderAnalysis.ClientOrder != null)
        {
            orderAnalysis.ClientOrderId = orderAnalysis.ClientOrder.ClientOrderId;
        }

        // Set the ClientOrder to null if ClientOrderId is not set
        orderAnalysis.ClientOrder = null;

        // Set the AnalysisId if Analysis is not null
        if (orderAnalysis.Analysis != null)
        {
            orderAnalysis.AnalysisId = orderAnalysis.Analysis.AnalysisId;
        }

        // Set the Analysis to null if AnalysisId is not set
        orderAnalysis.Analysis = null;

        // Set the OrderAnalysisId to 0 if it is not set
        if (orderAnalysis.OrderAnalysisId != 0)
        {
            orderAnalysis.OrderAnalysisId = 0;
        }

        orderAnalysis.CreateDatetime = existingEntity.CreateDatetime;
        orderAnalysis.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(orderAnalysis);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(orderAnalysis.OrderAnalysisId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.OrderAnalyses
            .Where(oa => oa.OrderAnalysisId == id)
            .Include(oa => oa.Analysis).ThenInclude(a => a.Category)
            .Include(oa => oa.Analysis).ThenInclude(a => a.AnalysisBiomaterials).ThenInclude(ab => ab.Biomaterial)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.OrderAnalyses.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.OrderAnalyses.RemoveRange(_aContext.OrderAnalyses);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}