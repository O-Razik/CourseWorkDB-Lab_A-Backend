using System.Security.Cryptography.X509Certificates;
using Bogus;
using Lab_A.Abstraction.IData;
using Lab_A.Abstraction.IModels;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.GEN.Generators;

public class ProcessDataGeneration : IGenerator<LabAContext>
{
    private int MonthAmount { get; init; }

    private readonly DateTime _cutoffDate;

    private int _inventoryOrderNumber;

    public BaseUnitOfWork<LabAContext> UnitOfWork { get; init; }

    public ProcessDataGeneration(BaseUnitOfWork<LabAContext> unitOfWork, int monthAmount)
    {
        UnitOfWork = unitOfWork;
        MonthAmount = monthAmount;
        _inventoryOrderNumber = 1;
        _cutoffDate = DateTime.Now;
    }

    public async Task Generate()
    {
        await StartProcess();

        Console.WriteLine("Start Process Finished!\n");

        await GeneratePerDay();

        Console.WriteLine("GeneratePerDay Process Finished\n");
    }

    private async Task StartProcess()
    {
        var laboratories = (await UnitOfWork.LaboratoryRepository.ReadAllAsync()).ToList();
        var inventory = (await UnitOfWork.InventoryRepository.ReadAllAsync()).ToList();
        var suppliers = (await UnitOfWork.SupplierRepository.ReadAllAsync()).ToList();

        var faker = new Faker("uk");
        var inventoryChunks = inventory.Chunk(3).ToArray();

        foreach (var t in inventoryChunks)
        {
            var supplier = faker.PickRandom(suppliers);

            var inventoryOrder = new InventoryOrder
            {
                Number = _inventoryOrderNumber,
                SupplierId = supplier.SupplierId,
                OrderDate = faker.Date.Between(this._cutoffDate.AddMonths(-MonthAmount - 1), this._cutoffDate.AddMonths(-MonthAmount)),
                StatusId = 3
            };
            _inventoryOrderNumber++;

            var inventoryInOrder = new List<IInventoryInOrder>();

            foreach (var inventoryChunk in t)
            {
                var amount = faker.Random.Number(500, 700);

                inventoryInOrder.Add(new InventoryInOrder()
                {
                    InventoryId = inventoryChunk.InventoryId,
                    Quantity = amount,
                    Price = inventoryChunk.Price * amount,
                });
            }

            inventoryOrder.Fullprice = inventoryInOrder.Sum(inOrder => inOrder.Price);

            inventoryOrder = (InventoryOrder)(await UnitOfWork.InventoryOrderRepository.CreateAsync(inventoryOrder));

            for (var index = 0; index < inventoryInOrder.Count; index++)
            {
                inventoryInOrder[index].InventoryOrderId = inventoryOrder.InventoryOrderId;
                var item = inventoryInOrder[index];
                inventoryInOrder[index] = await UnitOfWork.InventoryInOrderRepository.CreateAsync(item);
            }

            foreach (var item in inventoryInOrder)
            {
                var itemToLabAmount = item.Quantity / laboratories.Count;
                var itemToLabAmountRemainder = item.Quantity % laboratories.Count;

                foreach (var laboratory in laboratories)
                {
                    var inventoryDelivery = new InventoryDelivery
                    {
                        InventoryInOrderId = item.InventoryInOrderId,
                        Quantity = itemToLabAmount + (itemToLabAmountRemainder > 0 ? 1 : 0),
                        ExpirationDate = inventoryOrder.OrderDate?.AddYears(5),
                        StatusId = 3,
                        DeliveryDate = inventoryOrder.OrderDate?.AddDays(faker.Random.Number(0, 2))
                    };
                    itemToLabAmountRemainder--;

                    var inventoryInLab = new InventoryInLaboratory
                    {
                        LaboratoryId = laboratory.LaboratoryId,
                        InventoryId = item.InventoryId,
                        Quantity = inventoryDelivery.Quantity,
                        ExpirationDate = inventoryOrder.OrderDate?.AddYears(5),
                    };

                    inventoryInLab = (InventoryInLaboratory)(await UnitOfWork.InventoryInLaboratoryRepository.CreateAsync(inventoryInLab));

                    inventoryDelivery.InventoryInLaboratoryId = inventoryInLab.InventoryInLaboratoryId;

                    await UnitOfWork.InventoryDeliveryRepository.CreateAsync(inventoryDelivery);
                }
            }
        }
    }

    private async Task GeneratePerDay()
    {
        var laboratories = (await UnitOfWork.LaboratoryRepository.ReadAllAsync()).ToList();
        var employees = (await UnitOfWork.EmployeeRepository.ReadAllAsync()).ToList();
        Dictionary <ILaboratory, (List<IEmployee> employees, List<ISchedule> schedules)> laboratoryStaff = laboratories.ToDictionary(
            lab => lab,
            lab => (
                employees.Where(e => e.LaboratoryId == lab.LaboratoryId).ToList(),
                this.GetLabSchedules(lab).Result
            )
        );
        var suppliers = (await UnitOfWork.SupplierRepository.ReadAllAsync()).ToList();

        var clients = (await UnitOfWork.ClientRepository.ReadAllAsync()).ToList();
        var analyses = (await UnitOfWork.AnalysisRepository.ReadAllAsync()).ToList();
        var inventoryTypes = (await UnitOfWork.InventoryRepository.ReadAllAsync()).ToList();

        var inventoryDeliveries = (await UnitOfWork.InventoryDeliveryRepository.ReadAllAsync()).ToList();
        var lastDate = inventoryDeliveries.Max(x => x.DeliveryDate);
        var daysToCutOff = (this._cutoffDate - lastDate!.Value).Days;


        var faker = new Faker("uk");
        var orderNumber = 1;

        Console.WriteLine("Generation per day starts! TotalDays: " + daysToCutOff + ". \n");
        for (var i = 0; i < daysToCutOff; i++)
        {
            var today = DateOnly.FromDateTime(lastDate!.Value.AddDays(i));

            foreach (var lab in laboratoryStaff)
            {
                var biomaterialCollections = new List<BiomaterialCollection>();
                var orders = new List<ClientOrder>();
                var ordersAnalyses = new List<OrderAnalysis>();

                var labScheduleToday = lab.Value.schedules.Find(s => s.DayId == ((int)today.DayOfWeek + 1));
                if (labScheduleToday == null) continue;
                var ordersAmount = faker.Random.Number(0, 4);

                if (lab.Value.employees.Count == 0) continue;

                for (var j = 0; j < ordersAmount; j++)
                {
                    var orderTime = labScheduleToday!.StartTime!.Value.AddMinutes(faker.Random
                        .Number(0, ((labScheduleToday.CollectionEndTime - labScheduleToday.StartTime)!).Value.Minutes));

                    var randomEmployee = faker.Random.ListItem(employees);
                    var randomClient = faker.Random.ListItem(clients);

                    var clientOrder = new ClientOrder
                    {
                        Number = orderNumber,
                        StatusId = 3,
                        EmployeeId = randomEmployee.EmployeeId,
                        ClientId = randomClient.ClientId,
                        BiomaterialCollectionDate = today.ToDateTime(orderTime),
                        Fullprice = 0
                    }; 
                    orderNumber++;

                    var analysesAmount = faker.Random.Number(1, 5);
                    var orderAnalyses = new List<OrderAnalysis>();

                    for (var k = 0; k < analysesAmount; k++)
                    {
                        var analysis = faker.Random.ListItem(analyses);
                        var orderAnalysis = new OrderAnalysis
                        {
                            AnalysisId = analysis.AnalysisId,
                        };
                        orderAnalyses.Add(orderAnalysis);
                        clientOrder.Fullprice += analysis.Price;
                    }

                    clientOrder = (ClientOrder)(await UnitOfWork.ClientOrderRepository.CreateAsync(clientOrder));

                    foreach (var orderAnalysis in orderAnalyses)
                    {
                        orderAnalysis.ClientOrderId = clientOrder.ClientOrderId;
                        ordersAnalyses.Add(
                            (OrderAnalysis)await UnitOfWork.OrderAnalysisRepository.CreateAsync(orderAnalysis));
                    }

                    var biomaterialsNeeded = new List<Biomaterial>();

                    foreach (var analysis in orderAnalyses.Select(orderAnalysis => analyses.FirstOrDefault(a => a.AnalysisId == orderAnalysis.AnalysisId)).Where(analysis => analysis != null))
                    {
                        var analysisBiomaterials =
                            (await UnitOfWork.AnalysisBiomaterialRepository.ReadAllByAnalysisIdAsync(analysis.AnalysisId))
                            .ToList();

                        foreach (var analysisBiomaterial in analysisBiomaterials)
                        {
                            var biomaterial = await UnitOfWork.BiomaterialRepository.ReadAsync((int)analysisBiomaterial.BiomaterialId);
                            biomaterialsNeeded.Add(((Biomaterial)biomaterial)!);
                        }
                    }

                    var collectionDate = DateOnly.FromDateTime(clientOrder!.BiomaterialCollectionDate!.Value);
                    var expirationDate = new DateTime(collectionDate.AddYears(5).Year, collectionDate.AddYears(5).Month, collectionDate.AddYears(5).Day, 0, 0, 0, DateTimeKind.Utc);

                    foreach (var biomaterial in biomaterialsNeeded)
                    {
                        var inventoriesInLab = (await UnitOfWork.InventoryInLaboratoryRepository.GetByLaboratoryAsync(lab.Key.LaboratoryId)).ToList();
                        var inventoryInLab = GetInventoryForBiomaterial(inventoriesInLab, biomaterial)!;

                        if (inventoryInLab.Quantity <= 0)
                        {
                            clientOrder.StatusId = 4;
                            clientOrder = (ClientOrder)(await UnitOfWork.ClientOrderRepository.UpdateAsync(clientOrder))!;
                            break;
                        }

                        var biomaterialCollection = new BiomaterialCollection
                        {
                            ClientOrderId = clientOrder.ClientOrderId,
                            BiomaterialId = biomaterial.BiomaterialId,
                            InventoryInLaboratoryId = inventoryInLab.InventoryInLaboratoryId,
                            Volume = faker.Random.Number(100, 500),
                            CollectionDate = collectionDate,
                            ExpirationDate = expirationDate,
                        };

                        biomaterialCollection = (BiomaterialCollection)(await UnitOfWork.BiomaterialCollectionRepository.CreateAsync(biomaterialCollection));

                        biomaterialCollections.Add(biomaterialCollection);
                        
                        UnitOfWork.GetContext().Entry(inventoryInLab).State = EntityState.Detached;

                        inventoryInLab.Quantity -= 1;
                        await UnitOfWork.InventoryInLaboratoryRepository.UpdateAsync(inventoryInLab);
                    }

                    orders.Add(clientOrder);
                }

                if (orderNumber % 10 == 0 && !orders.IsNullOrEmpty())
                {
                    var canceledOrders = faker.Random.ListItem(orders);
                    canceledOrders.StatusId = 4;
                    await UnitOfWork.ClientOrderRepository.UpdateAsync(canceledOrders);
                }

                var deliveryDate = new DateTime(
                    today.Year,
                    today.Month,
                    today.Day,
                    labScheduleToday!.CollectionEndTime!.Value.Hour,
                    labScheduleToday.CollectionEndTime!.Value.Minute,
                    0,
                    DateTimeKind.Local).AddMinutes(
                        faker.Random.Number(
                            0,
                            ((labScheduleToday.EndTime - labScheduleToday.CollectionEndTime)!).Value.Minutes
                            ));


                var analysisCenter = await UnitOfWork.AnalysisCenterRepository.GetByCity(lab.Key.CityId);
                var biomaterialDeliveries = new List<BiomaterialDelivery>();

                foreach (var biomaterialCollection in biomaterialCollections)
                {
                    if (orders.Find(o => o.ClientOrderId == biomaterialCollection.ClientOrderId)!.StatusId == 4)
                    {
                        continue;
                    }

                    var status = 3;
                    if (i + 14 >= daysToCutOff)
                    {
                        status = 2;
                    }

                    if (i + 7 >= daysToCutOff)
                    {
                        status = 1;
                    }

                    var delivery = new BiomaterialDelivery()
                    {
                        BiomaterialCollectionId = biomaterialCollection.BiomaterialCollectionId,
                        DeliveryDate = deliveryDate,
                        AnalysisCenterId = analysisCenter.AnalysisCenterId,
                        StatusId = status
                    };
                    delivery =
                        (BiomaterialDelivery)(await UnitOfWork.BiomaterialDeliveryRepository.CreateAsync(delivery));
                    biomaterialDeliveries.Add(delivery);
                }

                if (i + 14 >= daysToCutOff)
                {
                    continue;
                }

                if (biomaterialDeliveries.Count > 6)
                {
                    var canceledDeliveries = faker.Random.ListItem(biomaterialDeliveries);
                    biomaterialDeliveries.Remove(canceledDeliveries);
                    canceledDeliveries.StatusId = 4;
                    await UnitOfWork.BiomaterialDeliveryRepository.UpdateAsync(canceledDeliveries);

                    var reDelivery = new BiomaterialDelivery
                    {
                        BiomaterialCollectionId = canceledDeliveries.BiomaterialCollectionId,
                        DeliveryDate = canceledDeliveries.DeliveryDate!.Value.AddDays(faker.Random.Number(1, 3)),
                        AnalysisCenterId = canceledDeliveries.AnalysisCenterId,
                        StatusId = 3
                    };

                    reDelivery =
                        (BiomaterialDelivery)(await UnitOfWork.BiomaterialDeliveryRepository.CreateAsync(reDelivery));
                    biomaterialDeliveries.Add(reDelivery);
                }

                var analysesToRemove = ordersAnalyses.Where(analysis => orders.Find(o => o.ClientOrderId == analysis.ClientOrderId)!.StatusId == 4).ToList();

                foreach (var analysis in analysesToRemove)
                {
                    ordersAnalyses.Remove(analysis);
                }

                foreach (var orderAnalysis in ordersAnalyses)
                {
                    var indicator = faker.Random.Number(0, 100);
                    var order = await UnitOfWork.ClientOrderRepository.ReadAsync((int)orderAnalysis.ClientOrderId!);

                    var analysisResult = new AnalysisResult()
                    {
                        AnalysisCenterId = analysisCenter?.AnalysisCenterId,
                        OrderAnalysisId = orderAnalysis.OrderAnalysisId,
                        ExecutionDate = order!.BiomaterialCollectionDate?.AddDays(faker.Random.Number(10, 12)),
                        Indicator = indicator,
                        Description = indicator > 50 ? $"Позитивний: {indicator} > 50" : $"Негативний: {indicator} < 50",
                    };

                    await UnitOfWork.AnalysisResultRepository.CreateAsync(analysisResult);
                }
            }

            if (i + 5 >= daysToCutOff)
            {
                continue;
            }

            var inventoryInLabs = (await UnitOfWork.InventoryInLaboratoryRepository.ReadAllAsync()).ToList();
            Dictionary<IInventory, List<int>> inventoryForOrder = new();

            foreach (var inv in inventoryTypes)
            {
                var labIds = inventoryInLabs.Select(x => x.LaboratoryId).ToHashSet();

                foreach (var lab in from lab in labIds 
                         let invInLab = inventoryInLabs.Where(x => x.InventoryId == inv.InventoryId && x.LaboratoryId == lab).ToList()
                         let hasInvEnded = invInLab.All(invLab => !(invLab.Quantity > 0))
                         where hasInvEnded 
                         select lab)
                {
                    if (inventoryForOrder.TryGetValue(inv, out var value))
                    {
                        value.Add(((int)lab)!);
                    }
                    else
                    {
                        inventoryForOrder.Add(inv, new List<int>());
                        inventoryForOrder[inv].Add((((int)lab)!));
                    }
                }
            }

            if (inventoryForOrder.Count > 0)
            {
                for (var j = 0; j < faker.Random.Number(1, 3); j++)
                {
                    var supplier = faker.Random.ListItem(suppliers);
                    var inventoryOrder = new InventoryOrder
                    {
                        Number = _inventoryOrderNumber,
                        SupplierId = supplier.SupplierId,
                        OrderDate = today.AddDays(1).ToDateTime(new TimeOnly(faker.Random.Number(8, 9), faker.Random.Number(0, 45))),
                        StatusId = 3
                    };
                    _inventoryOrderNumber++;

                    var quantity = faker.Random.Number(200, 400);
                    var inventoryInOrder = inventoryForOrder.Select(inventory => new InventoryInOrder()
                    {
                        InventoryId = inventory.Key.InventoryId,
                        Quantity = quantity,
                        Price = inventory.Key.Price * quantity,
                    }).ToList();

                    var fullPrice = inventoryInOrder.Sum(inOrder => inOrder.Price);
                    inventoryOrder.Fullprice = fullPrice;

                    inventoryOrder = (InventoryOrder)(await UnitOfWork.InventoryOrderRepository.CreateAsync(inventoryOrder));

                    for (var index = 0; index < inventoryInOrder.Count; index++)
                    {
                        inventoryInOrder[index].InventoryOrderId = inventoryOrder.InventoryOrderId;
                        var item = inventoryInOrder[index];
                        inventoryInOrder[index] = (InventoryInOrder)await UnitOfWork.InventoryInOrderRepository.CreateAsync(item);
                    }

                    var random = faker.Random.Number(1, 10);
                    if (random > 8)
                    {
                        inventoryOrder.StatusId = 4;
                        await UnitOfWork.InventoryOrderRepository.UpdateAsync(inventoryOrder);

                        supplier = faker.Random.ListItem(suppliers);

                        var reOrder = new InventoryOrder
                        {
                            Number = _inventoryOrderNumber,
                            SupplierId = supplier.SupplierId,
                            OrderDate = today.AddDays(1).ToDateTime(new TimeOnly(faker.Random.Number(10, 11), faker.Random.Number(0, 45))),
                            StatusId = 3,
                            Fullprice = fullPrice,
                        };
                        _inventoryOrderNumber++;

                        inventoryOrder = (InventoryOrder)await UnitOfWork.InventoryOrderRepository.CreateAsync(reOrder);

                        foreach (var item in inventoryInOrder)
                        {
                            item.InventoryOrderId = inventoryOrder.InventoryOrderId;
                        }

                        for (var index = 0; index < inventoryInOrder.Count; index++)
                        {
                            var item = new InventoryInOrder()
                            {
                                InventoryOrderId = inventoryOrder.InventoryOrderId,
                                InventoryId = inventoryInOrder[index].InventoryId,
                                Quantity = inventoryInOrder[index].Quantity,
                                Price = inventoryInOrder[index].Price,
                            };

                            inventoryInOrder[index] = (await UnitOfWork.InventoryInOrderRepository.CreateAsync(item) as InventoryInOrder)!;
                        }
                    }

                    if (i + 5 >= daysToCutOff)
                    {
                        continue;
                    }

                    foreach (var invForOrder in inventoryForOrder)
                    {
                        var invInOrder = inventoryInOrder.Find(x => x.InventoryId == invForOrder.Key.InventoryId);
                        var invPerLab = invInOrder!.Quantity / invForOrder.Value.Count;
                        var restInv = invInOrder.Quantity % invForOrder.Value.Count;

                        foreach (var laboratory in invForOrder.Value)
                        {
                            var inventoryDelivery = new InventoryDelivery
                            {
                                InventoryInOrderId = invInOrder.InventoryInOrderId,
                                Quantity = invPerLab + (restInv > 0 ? 1 : 0),
                                ExpirationDate = inventoryOrder.OrderDate?.AddYears(5),
                                StatusId = 3,
                                DeliveryDate = inventoryOrder.OrderDate?.AddDays(faker.Random.Number(1, 3))
                            };
                            restInv--;

                            var inventoryInLab = new InventoryInLaboratory
                            {
                                LaboratoryId = laboratory,
                                InventoryId = invForOrder.Key.InventoryId,
                                Quantity = inventoryDelivery.Quantity,
                                ExpirationDate = inventoryDelivery.ExpirationDate
                            };

                            inventoryInLab = (InventoryInLaboratory)(await UnitOfWork.InventoryInLaboratoryRepository.CreateAsync(inventoryInLab));

                            inventoryDelivery.InventoryInLaboratoryId = inventoryInLab.InventoryInLaboratoryId;

                            await UnitOfWork.InventoryDeliveryRepository.CreateAsync(inventoryDelivery);
                        }
                    }
                }
            }

            Console.WriteLine(" Generation for day " + today + " finished!\n " + (daysToCutOff - i) + " days to go!");
        }
    }

    private static readonly Dictionary<string, int[]> BiomaterialToInventory = new()
    {
        { "Венозна кров", new[] { 3 } }, // Голка, Шприц, Пробірка
        { "Капілярна кров", new[] { 2 } },
        { "Сеча", new[] { 4 } },
        { "Кал", new[] { 7 } },
        { "Зішкріб з періанальної області", new[] { 8 } },
        { "Зішкріб шкіри або вії", new[] { 8 } },
        { "Мазок з урогенітального тракту", new[] { 8 } },
        { "Мазок з шийки матки", new[] { 8 } },
        { "Зішкріб з урогенітального тракту", new[] { 8 } },
        { "Зішкріб з шийки матки", new[] { 8 } },
        { "Мазок із ротоглотки або носа", new[] { 8 } },
        { "Слиз із носоглотки", new[] { 8 } },
        { "Ізолят бактерій", new[] { 10 } },
        { "Зіскрібок з шийки матки", new[] { 6 } },
        { "Плевральна рідина", new[] { 3 } },
        { "Асцитична рідина", new[] { 3 } },
        { "Венозна кров матері", new[] { 3 } },
        { "Харкотиння", new [] { 3 } },
    };

    private async Task<List<ISchedule>> GetLabSchedules(ILaboratory laboratory)
    {
        var laboratorySchedules = (await UnitOfWork.LaboratoryScheduleRepository.ReadByLaboratoryAsync(laboratory.LaboratoryId));
        var scheduleIds = laboratorySchedules.Select(scheduleId => scheduleId.ScheduleId).Where(id => id != null).Cast<int>();
        var schedules = new List<ISchedule>();
        foreach (var scheduleId in scheduleIds)
        {
            var schedule = await UnitOfWork.ScheduleRepository.ReadAsync(scheduleId);
            if (schedule != null)
            {
                schedules.Add(schedule);
            }
        }
        return schedules;
    }

    private IInventoryInLaboratory? GetInventoryForBiomaterial(IEnumerable<IInventoryInLaboratory> inventoryInLaboratory, IBiomaterial biomaterial)
    {

        if (BiomaterialToInventory.TryGetValue(biomaterial.BiomaterialName, out var requiredInventoryIds))
        {
            foreach (var inventoryId in requiredInventoryIds)
            {
                var inventoryItem = inventoryInLaboratory.FirstOrDefault(inv => inv.InventoryId == inventoryId && inv.Quantity > 0);
                if (inventoryItem != null)
                {
                    return inventoryItem;
                }
            }
        }

        return null;
    }


}