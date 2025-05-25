using System.Diagnostics;
using Bogus;
using Lab_A.Abstraction.IData;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_A.GEN.Generators.ObjectGenerators;

public class InfrastructureGenerator : IGenerator<LabAContext>
{
    private int CityAmount { init; get; }

    private int MaxLabAmount { init; get; }

    private int SupplierAmount { init; get; }

    public BaseUnitOfWork<LabAContext> UnitOfWork { get; }

    public InfrastructureGenerator(BaseUnitOfWork<LabAContext> unitOfWork, int cityAmount, int maxLabAmount, int supplierAmount)
    {
        UnitOfWork = unitOfWork;
        CityAmount = cityAmount;
        MaxLabAmount = maxLabAmount;
        SupplierAmount = supplierAmount;
    }

    public async Task Generate()
    {
        await GenerateCities();
        await GenerateSchedules();
        await GenerateLaboratories();
        await GenerateLaboratorySchedules();
        await GenerateEmployees();
        await GenerateAnalysisCenter();
        await GenerateSuppliers();
    }

    private async Task GenerateCities()
    {
        var faker = new Faker("uk");

        for (int i = 0; i < CityAmount; i++)
        {
            var city = new City
            {
                CityName = faker.Address.City()
            };

            await UnitOfWork.CityRepository.CreateAsync(city);
        }
    }

    private async Task GenerateSchedules()
    {
        var faker = new Faker("uk");

        for (var i = 0; i < 7; i++)
        {
            var scheduleAmount = faker.Random.Number(1, 3);
            for (var j = 0; j < scheduleAmount; j++)
            {
                var startTime = faker.Date.Between(DateTime.Today.AddHours(7.5), DateTime.Today.AddHours(9));
                var collectionEndTime = faker.Date.Between(DateTime.Today.AddHours(11), DateTime.Today.AddHours(13));
                var endTime = faker.Date.Between(DateTime.Today.AddHours(16), DateTime.Today.AddHours(18));

                var schedule = new Schedule
                {
                    DayId = i + 1,
                    StartTime = TimeOnly.FromDateTime(startTime),
                    CollectionEndTime = TimeOnly.FromDateTime(collectionEndTime),
                    EndTime = TimeOnly.FromDateTime(endTime)
                };

                await UnitOfWork.ScheduleRepository.CreateAsync(schedule);
            }
        }
    }

    private async Task GenerateLaboratories()
    {
        var faker = new Faker("uk");
        
        for (var i = 0; i < CityAmount; i++)
        {
            var laboratoryAmount = faker.Random.Number(1, this.MaxLabAmount);
            for (var j = 0; j < laboratoryAmount; j++)
            {
                var laboratory = new Laboratory
                {
                    Address = faker.Address.StreetAddress(true),
                    CityId = i + 1,
                    PhoneNumber = faker.Phone.PhoneNumber("(+###)###-###-####")
                };

                await UnitOfWork.LaboratoryRepository.CreateAsync(laboratory);
            }
        }
    }

    private async Task GenerateLaboratorySchedules()
    {
        var faker = new Faker("uk");

        var laboratories = (await UnitOfWork.LaboratoryRepository.ReadAllAsync()).ToList();

        foreach (var laboratory in laboratories)
        {
            var schedules = await UnitOfWork.ScheduleRepository.ReadAllAsync();

            for (var i = 1; i <= 7; i++)
            {
                if (schedules == null) continue;

                var skip = faker.Random.Number(5);
                if (skip == 5) continue;

                var scheduleByOneDay = schedules.Where(s => s.DayId == i).ToList();
                var schedule = scheduleByOneDay[scheduleByOneDay.Count > 1 ? faker.Random.Number(0, scheduleByOneDay.Count - 1) : 0];

                var laboratorySchedule = new LaboratorySchedule
                {
                    LaboratoryId = laboratory.LaboratoryId,
                    ScheduleId = schedule.ScheduleId
                };

                await UnitOfWork.LaboratoryScheduleRepository.CreateAsync(laboratorySchedule);
            }
        }
    }

    private async Task GenerateEmployees()
    {
        var faker = new Faker("uk");

        var laboratories = (await UnitOfWork.LaboratoryRepository.ReadAllAsync()).ToList();
        var positions = (await UnitOfWork.PositionRepository.ReadAllAsync()).Select(p => p.PositionId).ToList();

        foreach (var laboratory in laboratories)
        {
            int employeeAmount = faker.Random.Number(4, 7);
            for (int i = 0; i < employeeAmount; i++)
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();

                var position = positions[1];

                if (i == 0)
                {
                    position = positions[0];
                }
                if (i + 2 == employeeAmount)
                {
                    position = positions[2];
                }
                if (i + 3 == employeeAmount)
                {
                    position = positions[3];
                }

                var employee = new Employee
                {
                    LaboratoryId = laboratory.LaboratoryId,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = faker.Phone.PhoneNumber("(+###)###-###-####"),
                    Email = faker.Internet.Email(firstName, lastName),
                    PositionId = position,
                };

                await UnitOfWork.EmployeeRepository.CreateAsync(employee);
            }
        }
    }

    public async Task GenerateAnalysisCenter()
    {
        var faker = new Faker("uk");

        foreach (var entity in await UnitOfWork.CityRepository.ReadAllAsync())
        {
            var center = new AnalysisCenter
            {
                CityId = entity.CityId,
                Address = faker.Address.StreetAddress(true)
            };

            await UnitOfWork.AnalysisCenterRepository.CreateAsync(center);
        }
    }

    private async Task GenerateSuppliers()
    {
        var faker = new Faker("uk");

        for (var i = 0; i < SupplierAmount; i++)
        {
            var supplier = new Supplier
            {
                Name = faker.Company.CompanyName(),
                Email = faker.Internet.Email(),
                License = faker.Random.Number(100000, 999999).ToString(),
                PhoneNumber = faker.Phone.PhoneNumber("(+###)###-###-####"),

            };
            await UnitOfWork.SupplierRepository.CreateAsync(supplier);
        }
    }
}