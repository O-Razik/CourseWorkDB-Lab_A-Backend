using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class PositionRepository : IPositionRepository
{
    private readonly LabAContext _aContext;
    public PositionRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IPosition> CreateAsync(IPosition entity)
    {
        var position = (Position)entity;

        // Set the PositionId to 0 if it is not set
        if (position.PositionId != 0)
        {
            position.PositionId = 0;
        }

        position.CreateDatetime = DateTime.Now;
        position.UpdateDatetime = null;

        await _aContext.Positions.AddAsync(position);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(position.PositionId))!;
    }

    public async Task<IPosition?> ReadAsync(int id)
    {
        return await _aContext.Positions.FindAsync(id);
    }

    public async Task<IEnumerable<IPosition>> ReadAllAsync()
    {
        return await _aContext.Positions.ToListAsync();
    }

    public async Task<IPosition?> UpdateAsync(IPosition entity)
    {
        var position = (Position)entity;
        var existingEntity = await _aContext.Positions.FindAsync(position.PositionId);
        if (existingEntity == null) return null;

        // Prevent updating the CreateDatetime
        position.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to the current time
        position.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(position);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(position.PositionId))!;
    }
        
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Positions.FindAsync(id);
        if (entity == null) return false;
        _aContext.Positions.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Positions.RemoveRange(_aContext.Positions);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}