using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace LabA.DAL.Repositories;

public class LaboratoryRepository : ILaboratoryRepository
{
    private readonly LabAContext _aContext;

    public LaboratoryRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<ILaboratory> CreateAsync(ILaboratory entity)
    {
        var laboratory = (Laboratory)entity;

        // Set the LaboratoryId to 0 if it is not set
        if (laboratory.LaboratoryId != 0)
        {
            laboratory.LaboratoryId = 0;
        }

        // Set the CityId if City is not null
        if (laboratory.City != null)
        {
            laboratory.CityId = laboratory.City.CityId;
        }

        // Set the City to null if CityId is not set
        laboratory.City = null;

        // Set the LaboratorySchedules to null if it is not set
        if (laboratory.LaboratorySchedules != null)
        {
            foreach (var laboratorySchedule in laboratory.LaboratorySchedules)
            {
                laboratorySchedule.Laboratory = null;
                if (laboratorySchedule.ScheduleId == 0)
                {
                    laboratorySchedule.Schedule = new Schedule
                    {
                        DayId = laboratorySchedule.Schedule.DayId,
                        StartTime = laboratorySchedule.Schedule.StartTime,
                        EndTime = laboratorySchedule.Schedule.EndTime,
                        CollectionEndTime = laboratorySchedule.Schedule.CollectionEndTime,
                        CreateDatetime = DateTime.Now,
                        UpdateDatetime = null,
                    };
                }
                else
                {
                    laboratorySchedule.Schedule = null;
                }
            }
        }

        laboratory.CreateDatetime = DateTime.Now;
        laboratory.UpdateDatetime = null;

        await _aContext.Laboratories.AddAsync(laboratory);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(laboratory.LaboratoryId))!;
    }

    public async Task<ILaboratory?> ReadAsync(int id)
    {
        return await _aContext.Laboratories.Where(l => l.LaboratoryId == id)
            .Include(l => l.City)
            .Include(l => l.LaboratorySchedules)
            .ThenInclude(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ILaboratory>> ReadAllAsync()
    {
        return await _aContext.Laboratories
            .Include(l => l.City)
            .Include(l => l.LaboratorySchedules)
            .ThenInclude(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .ToListAsync();
    }
    
public async Task<ILaboratory?> UpdateAsync(ILaboratory entity)
{
    var laboratory = (Laboratory)entity;
    var existingEntity = await _aContext.Laboratories
        .Include(l => l.LaboratorySchedules)
        .ThenInclude(ls => ls.Schedule)
        .FirstOrDefaultAsync(l => l.LaboratoryId == laboratory.LaboratoryId);

    if (existingEntity == null) return null;

    existingEntity.Address = laboratory.Address;
    existingEntity.PhoneNumber = laboratory.PhoneNumber;

    if (laboratory.City != null)
    {
        existingEntity.CityId = laboratory.City.CityId;
        existingEntity.City = await _aContext.Cities.FindAsync(laboratory.City.CityId);
    }

    foreach (var schedule in existingEntity.LaboratorySchedules.ToList())
    {
        _aContext.Remove(schedule);
    }

    existingEntity.UpdateDatetime = ValidateDateTime(DateTime.Now);
    if (existingEntity.CreateDatetime < new DateTime(1753, 1, 1))
        existingEntity.CreateDatetime = ValidateDateTime(DateTime.Now);

    if (laboratory.LaboratorySchedules != null)
    {
        foreach (var labSchedule in laboratory.LaboratorySchedules)
        {
            var schedule = labSchedule.Schedule;

            if (schedule.ScheduleId == 0)
            {
                schedule.ScheduleId = 0;
                schedule.CreateDatetime = ValidateDateTime(DateTime.Now);
                schedule.UpdateDatetime = null;
                schedule.DayId = schedule.Day?.DayId ?? 0;
                // Only set Day to null for new schedules to avoid tracking issues
                schedule.Day = null;

                await _aContext.Schedules.AddAsync(schedule);
                await _aContext.SaveChangesAsync();
            }
            else
            {
                schedule = await _aContext.Schedules.Include(s => s.Day).FirstOrDefaultAsync(s => s.ScheduleId == schedule.ScheduleId);
                if (schedule == null) continue;
                schedule.CreateDatetime = ValidateDateTime(schedule.CreateDatetime);
                schedule.UpdateDatetime = ValidateNullableDateTime(schedule.UpdateDatetime);
                // Do NOT set schedule.Day = null here!
            }

            var newLabSchedule = new LaboratorySchedule
            {
                LaboratoryId = existingEntity.LaboratoryId,
                ScheduleId = schedule.ScheduleId,
                Schedule = schedule,
                CreateDatetime = ValidateDateTime(DateTime.Now),
                UpdateDatetime = null
            };

            existingEntity.LaboratorySchedules.Add(newLabSchedule);
        }
    }

    try
    {
        await _aContext.SaveChangesAsync();
        return await this.ReadAsync(existingEntity.LaboratoryId);
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine($"Error saving changes: {ex}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException}");
        }
        throw;
    }
}
    // Helper methods for datetime validation
    private DateTime ValidateDateTime(DateTime dateTime)
    {
        return dateTime < new DateTime(1753, 1, 1) ? DateTime.Now : dateTime;
    } 

    private DateTime? ValidateNullableDateTime(DateTime? dateTime)
    {
        if (!dateTime.HasValue) return null;
        return dateTime.Value < new DateTime(1753, 1, 1) ? DateTime.Now : dateTime;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Laboratories
            .Where(l => l.LaboratoryId == id)
            .Include(l => l.City)
            .Include(l => l.LaboratorySchedules)
            .ThenInclude(ls => ls.Schedule)
            .ThenInclude(s => s.Day)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.Laboratories.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Laboratories.RemoveRange(_aContext.Laboratories);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}