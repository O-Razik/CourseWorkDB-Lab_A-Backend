using Lab_A.Abstraction.IData;
using Lab_A.DAL.Data;
using Lab_A.GEN.Generators.ObjectGenerators;

namespace Lab_A.GEN.Generators;

public class ObjectDataGenerator : IGenerator<LabAContext>
{
    public ObjectDataGenerator(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public BaseUnitOfWork<LabAContext> UnitOfWork { get; }

    public async Task Generate()
    {
        var staticDataGenerator = new StaticDataGenerator(UnitOfWork);
        await staticDataGenerator.Generate();
        
        var fromFilesGenerator = new FromFilesGenerator(UnitOfWork);
        await fromFilesGenerator.Generate();

        var departmentGenerator = new InfrastructureGenerator(UnitOfWork, 5, 3, 6);
        await departmentGenerator.Generate();

        Console.WriteLine("Generator ObjectDataGenerator finished");
    }
}