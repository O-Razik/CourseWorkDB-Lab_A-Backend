using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IBiomaterialRepository : ICrud<IBiomaterial>
{
    Task<IBiomaterial?> GetByName(string biomaterial);
}