using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class InventoryMapper
{
    public static InventoryDto ToDto(this IInventory inventory)
    {
        return new InventoryDto()
        {
            InventoryId = inventory.InventoryId,
            InventoryName = inventory.InventoryName,
            Price = (double)inventory.Price!,
        };
    }

    public static IInventory ToEntity(this InventoryDto inventoryDto)
    {
        return new Inventory
        {
            InventoryId = inventoryDto.InventoryId,
            InventoryName = inventoryDto.InventoryName,
            Price = inventoryDto.Price,
        };
    }
}