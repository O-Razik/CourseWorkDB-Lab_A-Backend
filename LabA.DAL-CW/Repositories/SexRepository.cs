using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class SexRepository : ISexRepository
{
    private readonly LabAContext _aContext;

    public SexRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }


    public async Task<ISex> CreateAsync(ISex entity)
    {
        var sex = (Sex)entity;

        // Set the SexId to 0 if it is not set
        if (sex.SexId != 0)
        {
            sex.SexId = 0;
        }

        sex.CreateDatetime = DateTime.Now;
        sex.UpdateDatetime = null;

        await _aContext.Sexes.AddAsync(sex);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(sex.SexId))!;
    }

    public async Task<ISex?> ReadAsync(int id)
    {
        return await _aContext.Sexes.FindAsync(id);
    }

    public async Task<IEnumerable<ISex>> ReadAllAsync()
    {
        return await _aContext.Sexes.ToListAsync();
    }

    public async Task<ISex?> UpdateAsync(ISex entity)
    {
        var sex = (Sex)entity;
        var existingSex = await _aContext.Sexes.FindAsync(entity.SexId);
        if (existingSex == null) return null;

        sex.CreateDatetime = existingSex.CreateDatetime;
        sex.UpdateDatetime = DateTime.Now;

        _aContext.Sexes.Update(sex);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(sex.SexId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Sexes.FindAsync(id);
        if (entity == null) return false;
        _aContext.Sexes.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Sexes.RemoveRange(_aContext.Sexes);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}