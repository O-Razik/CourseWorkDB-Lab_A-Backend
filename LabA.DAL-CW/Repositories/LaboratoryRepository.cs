using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

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
                laboratorySchedule.Schedule = null;
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
            .Where(l => l.LaboratoryId == laboratory.LaboratoryId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

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
                laboratorySchedule.Schedule = null;
            }
        }

        laboratory.CreateDatetime = existingEntity.CreateDatetime;
        laboratory.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(laboratory);
        await _aContext.SaveChangesAsync();
        return laboratory;
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