using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class AnalysisCategoryService : IAnalysisCategoryService
{
    private readonly IAnalysisCategoryRepository _repository;

    public AnalysisCategoryService(IAnalysisCategoryRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IAnalysisCategory> CreateAsync(IAnalysisCategory entity)
    {
        return await this._repository.CreateAsync(entity);
    }

    public async Task<IAnalysisCategory?> ReadAsync(int id)
    {
        return await this._repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IAnalysisCategory>> ReadAllAsync()
    {
        return await this._repository.ReadAllAsync();
    }

    public async Task<IAnalysisCategory?> UpdateAsync(IAnalysisCategory entity)
    {
        return await this._repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await this._repository.DeleteAsync(id);
    }
}