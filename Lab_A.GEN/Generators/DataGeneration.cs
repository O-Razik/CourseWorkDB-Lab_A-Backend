using Lab_A.Abstraction.IData;
using Lab_A.GEN.Generators.Clearer;
using Lab_A.DAL.Data;

namespace Lab_A.GEN.Generators;

public class DataGeneration : IGenerator<LabAContext>
{
    private readonly DataClearer _dataClearer;
    private readonly List<IGenerator<LabAContext>> generators = new List<IGenerator<LabAContext>>();

    public BaseUnitOfWork<LabAContext> UnitOfWork { get; }

    public DataGeneration(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
        _dataClearer = new DataClearer(unitOfWork);

        generators.Add(new ObjectDataGenerator(unitOfWork));
        generators.Add(new ProcessDataGeneration(unitOfWork, 3));
    }

    public async Task Generate()
    {
        await _dataClearer.Clear();

        foreach (var generator in generators)
        {
            await generator.Generate();
        }
    }
}