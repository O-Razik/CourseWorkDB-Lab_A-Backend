using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
    {
        _repository = repository;
    }

    public async Task<IClient> CreateAsync(IClient entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IClient?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IClient>> ReadAllAsync(
        int? sexId = null,
        string? search = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await _repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IClient>();

        // Sex filtering
        if (sexId.HasValue)
        {
            pipeline.Register(new EqualityStep<IClient, int?>(
                sexId,
                nameof(IClient.SexId)));
        }

        // Search filtering
        if (!string.IsNullOrEmpty(search))
        {
            pipeline.Register(new StringContainsStep<IClient>(
                search,
                nameof(IClient.FirstName),
                nameof(IClient.LastName),
                nameof(IClient.Email),
                nameof(IClient.PhoneNumber)));
        }

        // Apply all filters first
        query = pipeline.Execute(query);

        // Then apply paging
        var pagedQuery = new PagingStep<IClient>(pageNumber, pageSize)
            .Process(query);

        return pagedQuery.ToList();
    }

    public async Task<IClient?> UpdateAsync(IClient entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}