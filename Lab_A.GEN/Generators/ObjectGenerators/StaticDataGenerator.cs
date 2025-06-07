using Lab_A.Abstraction.IData;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;

namespace Lab_A.GEN.Generators.ObjectGenerators;

public class StaticDataGenerator : IGenerator<LabAContext>
{
    public BaseUnitOfWork<LabAContext> UnitOfWork { init; get; }

    public StaticDataGenerator(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task Generate()
    {
        await GenerateStatuses();
        await GenerateDays();
        await GeneratePositions();
        await GenerateSexes();
    }

    private async Task GenerateStatuses()
    {
        var statuses = new List<Status>
        {
            new Status { StatusName = "Новий" },
            new Status { StatusName = "В процесі" },
            new Status { StatusName = "Завершений" },
            new Status { StatusName = "Скасований" }
        };

        foreach (var status in statuses)
        {
            await UnitOfWork.StatusRepository.CreateAsync(status);
        }
    }

    private async Task GenerateDays()
    {
        var days = new List<Day>
        {
            new() { DayName = "Неділя" },
            new() { DayName = "Понеділок" },
            new() { DayName = "Вівторок" },
            new() { DayName = "Середа" },
            new() { DayName = "Четвер" },
            new() { DayName = "П'ятниця" },
            new() { DayName = "Субота" },
        };

        foreach (var day in days)
        {
            await UnitOfWork.DayRepository.CreateAsync(day);
        }
    }

    private async Task GeneratePositions()
    {
        var positions = new List<Position>
        {
            new() { PositionName = "Адміністратор" },
            new() { PositionName = "Реєстратор-касир" },
            new() { PositionName = "Менеджер інвентаря" },
            new() { PositionName = "Транспортер біоматеріалів" }
        };

        foreach (var position in positions)
        {
            await UnitOfWork.PositionRepository.CreateAsync(position);
        }
    }

    private async Task GenerateSexes()
    {
        await UnitOfWork.SexRepository.CreateAsync(new Sex()
        {
            SexName = "Male",
        });

        await UnitOfWork.SexRepository.CreateAsync(new Sex()
        {
            SexName = "Female",
        });
    }
}