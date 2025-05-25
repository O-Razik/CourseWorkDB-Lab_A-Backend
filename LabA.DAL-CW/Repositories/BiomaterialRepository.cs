using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class BiomaterialRepository : IBiomaterialRepository
{
    private readonly LabAContext _aContext;

    public BiomaterialRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IBiomaterial> CreateAsync(IBiomaterial entity)
    {
        var biomaterial = (Biomaterial)entity;

        biomaterial.CreateDatetime = DateTime.Now;
        biomaterial.UpdateDatetime = null;

        // Set the BiomaterialId to 0 if it is not set
        if (biomaterial.BiomaterialId != 0)
        {
            biomaterial.BiomaterialId = 0;
        }
        await _aContext.Biomaterials.AddAsync(biomaterial);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(biomaterial.BiomaterialId))!;
    }

    public async Task<IBiomaterial?> ReadAsync(int id)
    {
        return await _aContext.Biomaterials.FindAsync(id);
    }

    public async Task<IEnumerable<IBiomaterial>> ReadAllAsync()
    {
        return await _aContext.Biomaterials.ToListAsync();
    }

    public async Task<IBiomaterial?> UpdateAsync(IBiomaterial entity)
    {
        var biomaterial = (Biomaterial)entity;
        var existingEntity = await _aContext.Biomaterials.FindAsync(biomaterial.BiomaterialId);
        if (existingEntity == null) return null;

        biomaterial.UpdateDatetime = DateTime.Now;
        biomaterial.CreateDatetime = existingEntity.CreateDatetime;
        _aContext.Entry(existingEntity).CurrentValues.SetValues(biomaterial);
        await _aContext.SaveChangesAsync();
        return await ReadAsync(biomaterial.BiomaterialId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Biomaterials.FindAsync(id);
        if (entity == null) return false;
        _aContext.Biomaterials.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Biomaterials.RemoveRange(_aContext.Biomaterials);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IBiomaterial?> GetByName(string biomaterial)
    {
        var entity = await _aContext.Biomaterials.FirstOrDefaultAsync(c => c.BiomaterialName == biomaterial);
        return entity;
    }
}