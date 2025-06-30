using Lab_A.Abstraction.IModels;

namespace Lab_A.Abstraction.IRepositories;

public interface IClientOrderRepository : ICrud<IClientOrder>
{
    Task<IQueryable<IClientOrder>> QueryAsync();
    Task<IEnumerable<IClientOrder>> ReadAllByClientIdAsync(int clientId);
    
    Task<IClientOrder?> CancelOrderAsync(int clientOrderId);
}