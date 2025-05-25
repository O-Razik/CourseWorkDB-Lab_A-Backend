using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class InventoryOrderMapper
{
    public static InventoryOrderDto ToDto(this IInventoryOrder inventoryOrder)
    {
        return new InventoryOrderDto()
        {
            InventoryOrderId = inventoryOrder.InventoryOrderId,
            Number = (int)inventoryOrder.Number!,
            SupplierId = (int)inventoryOrder.SupplierId!,
            StatusId = inventoryOrder.StatusId,
            Fullprice = (double)inventoryOrder.Fullprice!,
            OrderDate = (DateTime)inventoryOrder.OrderDate!,
            Supplier = inventoryOrder.Supplier.ToDto(),
            Status = inventoryOrder.Status.ToDto(),
            InventoryInOrders = inventoryOrder.InventoryInOrders.Select(x => x.ToDto()).ToList(),
        };
    }

    public static IInventoryOrder ToEntity(this InventoryOrderDto inventoryOrderDto)
    {
        return new InventoryOrder
        {
            InventoryOrderId = inventoryOrderDto.InventoryOrderId,
            Number = inventoryOrderDto.Number,
            SupplierId = inventoryOrderDto.SupplierId,
            StatusId = inventoryOrderDto.StatusId,
            Fullprice = inventoryOrderDto.Fullprice,
            OrderDate = inventoryOrderDto.OrderDate,
            Supplier = (Supplier)inventoryOrderDto.Supplier.ToEntity(),
            Status = (Status)inventoryOrderDto.Status.ToEntity(),
            InventoryInOrders = inventoryOrderDto.InventoryInOrders.Select(x => (InventoryInOrder)x.ToEntity()).ToList(),
        };
    }
}