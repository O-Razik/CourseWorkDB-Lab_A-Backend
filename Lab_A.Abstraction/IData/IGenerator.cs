using Microsoft.EntityFrameworkCore;

namespace Lab_A.Abstraction.IData;

public interface IGenerator<T> where T : DbContext
{
    BaseUnitOfWork<T> UnitOfWork { get; }
    
    Task Generate();
}