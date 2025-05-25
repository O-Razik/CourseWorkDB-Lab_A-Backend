using Lab_A.Abstraction.IData;
using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.DAL.Data;

namespace Lab_A.BLL.Services;

public class AnalysisService : IAnalysisService
{
    private readonly IAnalysisRepository _repository;

    public AnalysisService(IAnalysisRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IAnalysis> CreateAsync(IAnalysis entity)
    {
        return await this._repository.CreateAsync(entity);
    }

    public async Task<IAnalysis?> ReadAsync(int id)
    {
        return await this._repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IAnalysis>> ReadAllAsync()
    {
        return await this._repository.ReadAllAsync();
    }

    public async Task<IAnalysis?> UpdateAsync(IAnalysis entity)
    {
        return await this._repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await this._repository.DeleteAsync(id);
    }
}