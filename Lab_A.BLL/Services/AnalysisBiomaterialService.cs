using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;

namespace Lab_A.BLL.Services;

public class AnalysisBiomaterialService : IAnalysisBiomaterialService
{
    private readonly IAnalysisBiomaterialRepository _repository;

    public AnalysisBiomaterialService(IAnalysisBiomaterialRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IAnalysisBiomaterial> CreateAsync(IAnalysisBiomaterial entity)
    {
        return await this._repository.CreateAsync(entity);
    }

    public async Task<IAnalysisBiomaterial?> ReadAsync(int id)
    {
        return await this._repository.ReadAsync(id);
    }

    public async Task<IEnumerable<IAnalysisBiomaterial>> ReadAllAsync()
    {
        return await this._repository.ReadAllAsync();
    }

    public async Task<IEnumerable<IAnalysisBiomaterial>> ReadAllByAnalysisIdAsync(int analysisId)
    {
        return await this._repository.ReadAllByAnalysisIdAsync(analysisId);
    }

    public async Task<IAnalysisBiomaterial?> UpdateAsync(IAnalysisBiomaterial entity)
    {
        return await this._repository.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await this._repository.DeleteAsync(id);
    }
}