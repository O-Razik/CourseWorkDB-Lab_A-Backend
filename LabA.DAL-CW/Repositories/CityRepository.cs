using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class CityRepository : ICityRepository
{
    private readonly LabAContext _aContext;

    public CityRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<ICity> CreateAsync(ICity entity)
    {
        var city = (City)entity;

        // Set the CityId to 0 if it is not set
        if (city.CityId != 0)
        {
            city.CityId = 0;
        }
        await _aContext.Cities.AddAsync(city);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(city.CityId))!;
    }

    public async Task<ICity?> ReadAsync(int id)
    {
        return await _aContext.Cities.FindAsync(id);
    }

    public async Task<IEnumerable<ICity>> ReadAllAsync()
    {
        return await _aContext.Cities.ToListAsync();
    }

    public async Task<ICity?> UpdateAsync(ICity entity)
    {
        var city = (City)entity;
        var existingEntity = await _aContext.Cities.FindAsync(city.CityId);
        if (existingEntity == null) return null;
        city.UpdateDatetime = DateTime.Now;
        city.CreateDatetime = existingEntity.CreateDatetime;
        _aContext.Entry(existingEntity).CurrentValues.SetValues(city);
        await _aContext.SaveChangesAsync();
        return await ReadAsync(city.CityId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Cities.FindAsync(id);
        if (entity == null) return false;
        _aContext.Cities.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Cities.RemoveRange(_aContext.Cities);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}