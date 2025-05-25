using System.Formats.Asn1;
using System.Globalization;
using Bogus;
using CsvHelper;
using CsvHelper.Configuration;
using Lab_A.Abstraction.IData;
using Lab_A.GEN.Models;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;

namespace Lab_A.GEN.Generators.ObjectGenerators.Writers;

public class ClientWriter
{
    // source: https://www.datablist.com/learn/csv/download-sample-csv-files
    private const string FilePath = "..\\..\\..\\Data\\clients_list.csv";

    public BaseUnitOfWork<LabAContext> UnitOfWork { get; }

    public ClientWriter(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task Write(IEnumerable<ClientCSV> list)
    {
        var faker = new Faker("uk");
        foreach (var client in list.Select(item => new Client()
                 {
                     FirstName = item.FirstName,
                     LastName = item.LastName,
                     SexId = item.Sex == "Male" ? 1 : 2,
                     Birthdate = !string.IsNullOrEmpty(item.Birthdate) ?
                         DateOnly.ParseExact(item.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture) : DateOnly.FromDateTime(DateTime.Today.AddYears(faker.Random.Int(-40, -10))),
                     PhoneNumber = item.PhoneNumber,
                     Email = item.Email
                 }))
        {
            await UnitOfWork.ClientRepository.CreateAsync(client);
        }
    }
}