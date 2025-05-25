using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly LabAContext _aContext;

    public SupplierRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<ISupplier> CreateAsync(ISupplier entity)
    {
        var supplier = (Supplier)entity;

        // Set the SupplierId to 0 if it is not set
        if (supplier.SupplierId != 0)
        {
            supplier.SupplierId = 0;
        }

        supplier.CreateDatetime = DateTime.Now;
        supplier.UpdateDatetime = null;

        await _aContext.Suppliers.AddAsync(supplier);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(supplier.SupplierId))!;
    }

    public async Task<ISupplier?> ReadAsync(int id)
    {
        return await _aContext.Suppliers.FindAsync(id);
    }

    public async Task<IEnumerable<ISupplier>> ReadAllAsync()
    {
        return await _aContext.Suppliers.ToListAsync();
    }

    public async Task<ISupplier?> UpdateAsync(ISupplier entity)
    {
        var supplier = (Supplier)entity;
        var existingEntity = await _aContext.Suppliers.FindAsync(supplier.SupplierId);
        if (existingEntity == null) return null;

        // Prevent updating the CreateDatetime
        supplier.CreateDatetime = existingEntity.CreateDatetime;
        // Set the UpdateDatetime to now
        supplier.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(supplier);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(supplier.SupplierId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Suppliers.FindAsync(id);
        if (entity == null) return false;
        _aContext.Suppliers.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Suppliers.RemoveRange(_aContext.Suppliers);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}