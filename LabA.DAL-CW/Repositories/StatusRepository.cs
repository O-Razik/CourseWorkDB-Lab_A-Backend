using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly LabAContext _aContext;

    public StatusRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IStatus> CreateAsync(IStatus entity)
    {
        var status = (Status)entity;

        // Set the StatusId to 0 if it is not set
        if (status.StatusId != 0)
        {
            status.StatusId = 0;
        }

        status.CreateDatetime = DateTime.Now;
        status.UpdateDatetime = null;

        await _aContext.Statuses.AddAsync(status);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(status.StatusId))!;
    }

    public async Task<IStatus?> ReadAsync(int id)
    {
        return await _aContext.Statuses.FindAsync(id);
    }

    public async Task<IEnumerable<IStatus>> ReadAllAsync()
    {
        return await _aContext.Statuses.ToListAsync();
    }

    public async Task<IStatus?> UpdateAsync(IStatus entity)
    {
        var status = (Status)entity;
        var existingEntity = await _aContext.Statuses.FindAsync(status.StatusId);
        if (existingEntity == null) return null;

        status.CreateDatetime = existingEntity.CreateDatetime;
        status.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(status);
        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(status.StatusId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Statuses.FindAsync(id);
        if (entity == null) return false;
        _aContext.Statuses.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Statuses.RemoveRange(_aContext.Statuses);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}