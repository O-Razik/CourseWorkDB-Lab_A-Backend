using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IEmployeeRepository : ICrud<IEmployee>
{
    Task<IEnumerable<IEmployee>> GetByLaboratoryAsync(int laboratoryId);
    Task<IQueryable<IEmployee>> Query();
}