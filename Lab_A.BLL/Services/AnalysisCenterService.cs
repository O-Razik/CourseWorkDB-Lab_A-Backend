using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class AnalysisCenterService : IAnalysisCenterService
{
    private readonly IAnalysisCenterRepository _repository;

    public AnalysisCenterService(IAnalysisCenterRepository repository)
    {
        _repository = repository;
    }

    public async Task<IAnalysisCenter> CreateAsync(IAnalysisCenter entity)
    {
        return await _repository.CreateAsync(entity);
    }

    public async Task<IAnalysisCenter?> ReadAsync(int id)
    {
        return await _repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IAnalysisCenter>> ReadAllAsync()
    {
        return await _repository.ReadAllAsync();
    }

    public async Task<IAnalysisCenter?> UpdateAsync(IAnalysisCenter entity)
    {
        return await _repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}