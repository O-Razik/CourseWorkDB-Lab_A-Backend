using CsvHelper.Configuration;
using CsvHelper;
using Lab_A.Abstraction.IData;
using Lab_A.GEN.Models;
using Lab_A.DAL.Data;
using Newtonsoft.Json;
using System.Globalization;
using Lab_A.GEN.Generators.ObjectGenerators.Writers;

namespace Lab_A.GEN.Generators.ObjectGenerators;

public class FromFilesGenerator : IGenerator<LabAContext>
{
    public BaseUnitOfWork<LabAContext> UnitOfWork { get; init; }

    public FromFilesGenerator(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task Generate()
    {
        await GenerateClients();
        await GenerateInventory();
        await GenerateAnalyses();
    }

    private async Task GenerateAnalyses()
    {
        var analyses = ReadFromJson<AnalysisJson>("Data/analyses_list.txt");
        var analysisWriter = new AnalysisWriter(UnitOfWork);
        await analysisWriter.Write(analyses);
    }

    private async Task GenerateInventory()
    {
        var inventoryList = ReadFromJson<InventoryJson>("Data/inventory_list.txt");
        var inventoryWriter = new InventoryWriter(UnitOfWork);
        await inventoryWriter.Write(inventoryList);
    }

    private async Task GenerateClients()
    {
        var clients = ReadFromCsv<ClientCSV>("Data/clients_list.csv");
        var clientWriter = new ClientWriter(UnitOfWork);
        await clientWriter.Write(clients);
    }

    private static List<T> ReadFromJson<T>(string filePath)
    {
        var result = new List<T>();

        try
        {
            var jsonContent = File.ReadAllText(filePath);
            result = JsonConvert.DeserializeObject<List<T>>(jsonContent) ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading or parsing file: {ex.Message}");
        }

        return result;
    }

    private static List<T> ReadFromCsv<T>(string filePath)
    {
        var result = new List<T>();
        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            result = csv.GetRecords<T>().ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading or parsing file: {ex.Message}");
        }
        return result;
    }
}