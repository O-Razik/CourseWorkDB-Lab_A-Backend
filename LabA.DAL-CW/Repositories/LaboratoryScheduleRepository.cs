using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class LaboratoryScheduleRepository : ILaboratoryScheduleRepository
{
    private readonly LabAContext _aContext;
    public LaboratoryScheduleRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }
    
    public async Task<ILaboratorySchedule> CreateAsync(ILaboratorySchedule entity)
    {
        var laboratorySchedule = (LaboratorySchedule)entity;

        // Set the LaboratoryScheduleId to 0 if it is not set
        if (laboratorySchedule.LaboratoryScheduleId != 0)
        {
            laboratorySchedule.LaboratoryScheduleId = 0;
        }

        // Set the LaboratoryId if Laboratory is not null
        if (laboratorySchedule.Laboratory != null)
        {
            laboratorySchedule.LaboratoryId = laboratorySchedule.Laboratory.LaboratoryId;
        }

        // Set the Laboratory to null if LaboratoryId is not set
        laboratorySchedule.Laboratory = null;

        // Set the ScheduleId if Schedule is not null
        if (laboratorySchedule.Schedule != null)
        {
            laboratorySchedule.ScheduleId = laboratorySchedule.Schedule.ScheduleId;
        }

        // Set the Schedule to null if ScheduleId is not set
        laboratorySchedule.Schedule = null;

        // Set the CreateDatetime to now
        laboratorySchedule.CreateDatetime = DateTime.Now;
        // Set the UpdateDatetime to null
        laboratorySchedule.UpdateDatetime = null;

        await _aContext.LaboratorySchedules.AddAsync(laboratorySchedule);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(laboratorySchedule.LaboratoryScheduleId))!;
    }
    
    public async Task<ILaboratorySchedule?> ReadAsync(int id)
    {
        return await _aContext.LaboratorySchedules
            .Where(ls => ls.LaboratoryScheduleId == id)
            .Include(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<ILaboratorySchedule>> ReadAllAsync()
    {
        return await _aContext.LaboratorySchedules
            .Include(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .ToListAsync();
    }

    public async Task<ILaboratorySchedule?> UpdateAsync(ILaboratorySchedule entity)
    {
        var laboratorySchedule = (LaboratorySchedule)entity;
        var existingEntity = await _aContext.LaboratorySchedules
            .Where(ls => ls.LaboratoryScheduleId == laboratorySchedule.LaboratoryScheduleId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the LaboratoryScheduleId to 0 if it is not set
        if (laboratorySchedule.LaboratoryScheduleId != 0)
        {
            laboratorySchedule.LaboratoryScheduleId = 0;
        }

        // Set the LaboratoryId if Laboratory is not null
        if (laboratorySchedule.Laboratory != null)
        {
            laboratorySchedule.LaboratoryId = laboratorySchedule.Laboratory.LaboratoryId;
        }

        // Set the Laboratory to null if LaboratoryId is not set
        laboratorySchedule.Laboratory = null;

        // Set the ScheduleId if Schedule is not null
        if (laboratorySchedule.Schedule != null)
        {
            laboratorySchedule.ScheduleId = laboratorySchedule.Schedule.ScheduleId;
        }

        // Set the Schedule to null if ScheduleId is not set
        laboratorySchedule.Schedule = null;

        // Prevent setting the CreateDatetime and UpdateDatetime to null
        laboratorySchedule.CreateDatetime = existingEntity.CreateDatetime;
        laboratorySchedule.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(laboratorySchedule);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(laboratorySchedule.LaboratoryScheduleId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.LaboratorySchedules
            .Where(ls => ls.LaboratoryScheduleId == id)
            .Include(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.LaboratorySchedules.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.LaboratorySchedules.RemoveRange(_aContext.LaboratorySchedules);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<ILaboratorySchedule>> ReadByLaboratoryAsync(int laboratoryId)
    {
        return await _aContext.LaboratorySchedules
            .Where(ls => ls.LaboratoryId == laboratoryId)
            .Include(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .ToListAsync();
    }
}