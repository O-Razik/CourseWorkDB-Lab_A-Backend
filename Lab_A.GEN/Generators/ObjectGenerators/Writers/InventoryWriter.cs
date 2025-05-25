using Lab_A.Abstraction.IData;
using Lab_A.GEN.Models;
using Lab_A.DAL.Data;
using Lab_A.DAL.Models;
using Newtonsoft.Json;

namespace Lab_A.GEN.Generators.ObjectGenerators.Writers;

public class InventoryWriter
{
    public BaseUnitOfWork<LabAContext> UnitOfWork { get; }

    public InventoryWriter(BaseUnitOfWork<LabAContext> unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task Write(IEnumerable<InventoryJson> inventoryList)
    {
        foreach (var inventory in inventoryList.Select(item => new Inventory()
                 {
                     InventoryName = item.InventoryName,
                     Price = item.Price
                 }))
        {
            await UnitOfWork.InventoryRepository.CreateAsync(inventory);
        }
    }
}