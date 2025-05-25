using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly LabAContext _aContext;

    public EmployeeRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IEmployee> CreateAsync(IEmployee entity)
    {
        var employee = (Employee)entity;
        // Set the EmployeeId to 0 if it is not set
        if (employee.EmployeeId != 0)
        {
            employee.EmployeeId = 0;
        }

        // Set the LaboratoryId if Laboratory is not null
        if (employee.Laboratory != null)
        {
            employee.LaboratoryId = employee.Laboratory.LaboratoryId;
        }

        // Set the Laboratory to null if LaboratoryId is not set
        employee.Laboratory = null;

        // Set the PositionId if Position is not null
        if (employee.Position != null)
        {
            employee.PositionId = employee.Position.PositionId;
        }

        // Set the Position to null if PositionId is not set
        employee.Position = null;

        employee.CreateDatetime = DateTime.Now;
        employee.UpdateDatetime = null;
        await _aContext.Employees.AddAsync(employee);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(employee.EmployeeId))!;
    }

    public async Task<IEmployee?> ReadAsync(int id)
    {
        return await _aContext.Employees
            .Where(em => em.EmployeeId == id)
            .Include(e => e.Laboratory).ThenInclude(l => l.City)
            .Include(e => e.Position)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IEmployee>> ReadAllAsync()
    {
        return await _aContext.Employees
            .Include(e => e.Laboratory).ThenInclude(l => l.City)
            .Include(e => e.Position)
            .ToListAsync();
    }

    public Task<IQueryable<IEmployee>> Query()
    {
        var query = _aContext.Employees
            .Include(e => e.Laboratory).ThenInclude(l => l.City)
            .Include(e => e.Position)
            .Select(e => (IEmployee)e)
            .AsQueryable();

        return Task.FromResult(query);
    }

    public async Task<IEmployee?> UpdateAsync(IEmployee entity)
    {
        var employee = (Employee)entity;
        var existingEntity = await _aContext.Employees
            .Where(em => em.EmployeeId == employee.EmployeeId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        // Set the LaboratoryId if Laboratory is not null
        if (employee.Laboratory != null)
        {
            employee.LaboratoryId = employee.Laboratory.LaboratoryId;
        }
        // Set the Laboratory to null if LaboratoryId is not set
        employee.Laboratory = null;

        // Set the PositionId if Position is not null
        if (employee.Position != null)
        {
            employee.PositionId = employee.Position.PositionId;
        }

        // Set the Position to null if PositionId is not set
        employee.Position = null;

        // Prevent updating the CreateDatetime
        // Set the UpdateDatetime to the current time
        employee.CreateDatetime = existingEntity.CreateDatetime;
        employee.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(employee);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(employee.EmployeeId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Employees
            .Where(em => em.EmployeeId == id)
            .Include(e => e.Laboratory).ThenInclude(l => l.City)
            .Include(e => e.Position)
            .FirstOrDefaultAsync();
        if (entity == null) return false;
        _aContext.Employees.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Employees.RemoveRange(_aContext.Employees);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<IEmployee>> GetByLaboratoryAsync(int laboratoryId)
    {
        return await _aContext.Employees
            .Where(em => em.EmployeeId == laboratoryId)
            .Include(e => e.Laboratory).ThenInclude(l => l.City)
            .Include(e => e.Position)
            .ToListAsync();
    }
}