using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class InventoryDeliveryMapper
{
    public static InventoryDeliveryDto ToDto(this IInventoryDelivery entity)
    {
        return new InventoryDeliveryDto
        {
            InventoryDeliveryId = entity.InventoryDeliveryId,
            InventoryInOrderId = (int)entity.InventoryInOrderId!,
            InventoryInLaboratoryId = (int)entity.InventoryInLaboratoryId!,
            Quantity = (int)entity.Quantity!,
            DeliveryDate = (DateTime)entity.DeliveryDate!,
            ExpirationDate = entity.ExpirationDate,
            StatusId = entity.StatusId,
            InventoryInLaboratory = entity.InventoryInLaboratory.ToDto(),
            LaboratoryFullAddress = entity.InventoryInLaboratory.Laboratory.City.CityName + ", " + entity.InventoryInLaboratory.Laboratory.Address,
            Status = entity.Status.ToDto(),
        };
    }

    public static IInventoryDelivery ToEntity(this InventoryDeliveryDto dto)
    {
        return new InventoryDelivery
        {
            InventoryDeliveryId = dto.InventoryDeliveryId,
            InventoryInOrderId = dto.InventoryInOrderId,
            InventoryInLaboratoryId = dto.InventoryInLaboratoryId,
            Quantity = dto.Quantity,
            DeliveryDate = dto.DeliveryDate,
            ExpirationDate = dto.ExpirationDate,
            StatusId = dto.StatusId,
            InventoryInLaboratory = (InventoryInLaboratory)dto.InventoryInLaboratory.ToEntity(),
            Status = (Status)dto.Status.ToEntity(),
        };
    }
}