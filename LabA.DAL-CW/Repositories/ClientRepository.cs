using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LabA.DAL.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly LabAContext _aContext;
    public ClientRepository(LabAContext aContext)
    {
        _aContext = aContext;
    }

    public async Task<IClient> CreateAsync(IClient entity)
    {
        var client = (Client)entity;

        // Set the ClientId to 0 if it is not set
        if (client.ClientId != 0)
        {
            client.ClientId = 0;
        }

        if (client.Sex != null)
        {
            client.SexId = client.Sex.SexId;
        }
        client.Sex = null;

        client.CreateDatetime = DateTime.Now;
        client.UpdateDatetime = null;

        await _aContext.Clients.AddAsync(client);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(client.ClientId))!;
    }

    public async Task<IClient?> ReadAsync(int id)
    {
        return await _aContext.Clients
            .Where(cl => cl.ClientId == id)
            .Include(cl => cl.Sex)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IClient>> ReadAllAsync()
    {
        return await _aContext.Clients
            .Include(cl => cl.Sex)
            .ToListAsync();
    }

    public async Task<IClient?> UpdateAsync(IClient entity)
    {
        var client = (Client)entity;
        var existingEntity = await _aContext.Clients
            .Where(cl => cl.ClientId == client.ClientId)
            .FirstOrDefaultAsync();
        if (existingEntity == null) return null;

        if (client.Sex != null)
        {
            client.SexId = client.Sex.SexId;
        }
        client.Sex = null;

        // Prevent updating the CreateDatetime
        client.CreateDatetime = existingEntity.CreateDatetime;

        // Set the UpdateDatetime to the current time
        client.UpdateDatetime = DateTime.Now;

        _aContext.Entry(existingEntity).CurrentValues.SetValues(client);
        await _aContext.SaveChangesAsync();
        return (await this.ReadAsync(client.ClientId))!;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _aContext.Clients
            .Where(cl => cl.ClientId == id)
            .Include(cl => cl.Sex)
            .FirstOrDefaultAsync();
        if (entity == null) return false;

        _aContext.Clients.Remove(entity);
        await _aContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllAsync()
    {
        try
        {
            _aContext.Clients.RemoveRange(_aContext.Clients);
            await _aContext.SaveChangesAsync();
            return true;
        }
        catch (Exception) {
            return false;
        }
    }
}