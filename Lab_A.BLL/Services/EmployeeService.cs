using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Pipeline;

namespace Lab_A.BLL.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEmployee> CreateAsync(IEmployee entity)
    {
        return await this._repository.CreateAsync(entity);
    }

    public async Task<IEmployee?> ReadAsync(int id)
    {
        return await this._repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IEmployee>> ReadAllAsync(string? search = null,
        int? laboratoryId = null,
        List<int>? positionIds = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = (await this._repository.ReadAllAsync()).AsQueryable();
        var pipeline = new Pipeline<IEmployee>();
        
        // Search filtering
        if (!string.IsNullOrEmpty(search))
        {
            pipeline.Register(new StringContainsStep<IEmployee>(
                search,
                nameof(IEmployee.FirstName),
                nameof(IEmployee.LastName),
                nameof(IEmployee.Email),
                nameof(IEmployee.PhoneNumber)));
        }
        
        // Laboratory filtering
        if (laboratoryId.HasValue)
        {
            pipeline.Register(new EqualityStep<IEmployee, int?>(
                laboratoryId,
                nameof(IEmployee.LaboratoryId)));
        }
        
        // Position filtering
        if (positionIds != null)
        {
            var ids = positionIds.ToList();
            if (ids.Any())
            {
                pipeline.Register(new ClassificationStep<IEmployee> (
                    ids,
                    nameof(IEmployee.PositionId)));
            }
        }

        // Apply all filters first
        var filteredQuery = pipeline.Execute(query);
        
        // Then apply paging
        var pagedQuery = new PagingStep<IEmployee>(pageNumber, pageSize)
            .Process(filteredQuery);
        
        return pagedQuery.ToList();
    }

    public async Task<IEmployee?> UpdateAsync(IEmployee entity)
    {
        return await this._repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await this._repository.DeleteAsync(id);
    }
}