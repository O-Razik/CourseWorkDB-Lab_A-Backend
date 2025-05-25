using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.BLL.DtoMappers;

public static class InventoryInOrderMapper
{
    public static InventoryInOrderDto ToDto(this IInventoryInOrder inventoryInOrder)
    {
        return new InventoryInOrderDto()
        {
            InventoryInOrderId = inventoryInOrder.InventoryInOrderId,
            InventoryOrderId = (int)inventoryInOrder.InventoryOrderId!,
            OrderNumber = (int)inventoryInOrder.InventoryOrder?.Number!,
            InventoryId = (int)inventoryInOrder.InventoryId!,
            Quantity = (int)inventoryInOrder.Quantity!,
            Price = (double)inventoryInOrder.Price!,
            Inventory = inventoryInOrder.Inventory.ToDto(),
            InventoryDeliveries = (inventoryInOrder.InventoryDeliveries.IsNullOrEmpty()) ? null : inventoryInOrder.InventoryDeliveries.Select(x => x.ToDto()).ToList(),
        };
    }

    public static IInventoryInOrder ToEntity(this InventoryInOrderDto inventoryInOrderDto)
    {
        return new InventoryInOrder()
        {
            InventoryInOrderId = inventoryInOrderDto.InventoryInOrderId,
            InventoryOrderId = inventoryInOrderDto.InventoryOrderId,
            InventoryId = inventoryInOrderDto.InventoryId,
            Quantity = inventoryInOrderDto.Quantity,
            Price = inventoryInOrderDto.Price,
            Inventory = (Inventory)inventoryInOrderDto.Inventory.ToEntity(),
            InventoryDeliveries = inventoryInOrderDto.InventoryDeliveries.Select(x => (InventoryDelivery)x.ToEntity()).ToList(),
        };
    }
}