using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class DayRepository : IDayRepository
{
    private readonly LabAContext _aContext;

    public DayRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IDay> CreateAsync(IDay entity)
    {
        var day = (Day)entity;
        day.CreateDatetime = DateTime.Now;
        day.UpdateDatetime = null;
        await _aContext.Days.AddAsync(day);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(day.DayId))!;
    }

    public async Task<IDay?> ReadAsync(int id)
    {
        return await _aContext.Days.FindAsync(id);
    }

    public async Task<IEnumerable<IDay>> ReadAllAsync()
    {
        return await _aContext.Days.ToListAsync();
    }

    public async Task<IDay?> UpdateAsync(IDay entity)
    {
        var day = (Day)entity;
        var existingEntity = await _aContext.Days.FindAsync(day.DayId);
        if (existingEntity == null) return null;
        // Prevent updating the CreateDatetime
        day.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to the current time
        day.UpdateDatetime = DateTime.Now;
        _aContext.Entry(existingEntity).CurrentValues.SetValues(day);
        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(day.DayId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Days.FindAsync(id);
        if (entity == null) return false;
        _aContext.Days.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Days.RemoveRange(_aContext.Days);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}