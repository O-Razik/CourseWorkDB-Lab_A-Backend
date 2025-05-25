using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class OrderAnalysisService : IOrderAnalysisService
{
    private readonly IOrderAnalysisRepository _repository;

    public OrderAnalysisService(IOrderAnalysisRepository repository)
    {
        _repository = repository;
    }

    public async Task<IOrderAnalysis> CreateAsync(IOrderAnalysis entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IOrderAnalysis?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IOrderAnalysis>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IEnumerable<IOrderAnalysis>> ReadAllByClientOrderIdAsync(int clientOrderId)
    {
        return await _repository.ReadAllByClientOrderAsync(clientOrderId);
    }

    public async Task<IOrderAnalysis?> UpdateAsync(IOrderAnalysis entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}