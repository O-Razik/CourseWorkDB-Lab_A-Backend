using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class InventoryInLaboratoryMapper
{
    public static InventoryInLaboratoryDto? ToDto(this IInventoryInLaboratory? inventoryInLaboratory)
    {
        if (inventoryInLaboratory == null)
        {
            return null;
        }
        
        return new InventoryInLaboratoryDto()
        {
            InventoryInLaboratoryId = inventoryInLaboratory.InventoryInLaboratoryId,
            LaboratoryId = (int)inventoryInLaboratory.LaboratoryId!,
            InventoryId = (int)inventoryInLaboratory.InventoryId!,
            Quantity = (int)inventoryInLaboratory.Quantity!,
            ExpirationDate = (DateTime)inventoryInLaboratory.ExpirationDate!,
            Inventory = inventoryInLaboratory.Inventory.ToDto()
        };
    }
    public static IInventoryInLaboratory ToEntity(this InventoryInLaboratoryDto? inventoryInLaboratoryDto)
    {
        return new InventoryInLaboratory
        {
            InventoryInLaboratoryId = inventoryInLaboratoryDto.InventoryInLaboratoryId,
            LaboratoryId = inventoryInLaboratoryDto.LaboratoryId,
            InventoryId = inventoryInLaboratoryDto.InventoryId,
            Quantity = inventoryInLaboratoryDto.Quantity,
            ExpirationDate = inventoryInLaboratoryDto.ExpirationDate,
            Inventory = (Inventory)inventoryInLaboratoryDto.Inventory.ToEntity()
        };
    }
}