using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly LabAContext _aContext;
    public ScheduleRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }
    public async Task<ISchedule> CreateAsync(ISchedule entity)
    {
        var schedule = (Schedule)entity;

        // Set the ScheduleId to 0 if it is not set
        if (schedule.ScheduleId != 0)
        {
            schedule.ScheduleId = 0;
        }

        // Set the DayId if Day is not null
        if (schedule.Day != null)
        {
            schedule.DayId = schedule.Day.DayId;
        }

        // Set the Day to null if DayId is not set
        schedule.Day = null;

        // Set the CreateDatetime to now
        schedule.CreateDatetime = DateTime.Now;
        // Set the UpdateDatetime to null
        schedule.UpdateDatetime = null;

        await _aContext.Schedules.AddAsync(schedule);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(schedule.ScheduleId))!;
    }
    public async Task<ISchedule?> ReadAsync(int id)
    {
        return await _aContext.Schedules
            .Where(s => s.ScheduleId == id)
            .Include(s => s.Day)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<ISchedule>> ReadAllAsync()
    {
        return await _aContext.Schedules
            .Include(s => s.Day)
            .ToListAsync();
    }
    public async Task<ISchedule?> UpdateAsync(ISchedule entity)
    {
        var schedule = (Schedule)entity;
        var existingEntity = await _aContext.Schedules
            .Where(s => s.ScheduleId == schedule.ScheduleId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the DayId if Day is not null
        if (schedule.Day != null)
        {
            schedule.DayId = schedule.Day.DayId;
        }

        // Set the Day to null if DayId is not set
        schedule.Day = null;

        // Set the UpdateDatetime to now
        schedule.UpdateDatetime = DateTime.Now;
        schedule.CreateDatetime = existingEntity.CreateDatetime;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(schedule);
        await _aContext.SaveChangesAsync();
        return schedule;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Schedules
            .Where(s => s.ScheduleId == id)
            .Include(s => s.Day)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.Schedules.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Schedules.RemoveRange(_aContext.Schedules);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}