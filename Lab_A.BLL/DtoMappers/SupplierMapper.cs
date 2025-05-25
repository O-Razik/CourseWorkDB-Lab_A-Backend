using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class SupplierMapper
{
    public static SupplierDto ToDto(this ISupplier supplier)
    {
        return new SupplierDto()
        {
            SupplierId = supplier.SupplierId,
            Name = supplier.Name,
            License = supplier.License,
            PhoneNumber = supplier.PhoneNumber,
            Email = supplier.Email,
        };
    }
    public static ISupplier ToEntity(this SupplierDto supplierDto)
    {
        return new Supplier
        {
            SupplierId = supplierDto.SupplierId,
            Name = supplierDto.Name,
            License = supplierDto.License,
            PhoneNumber = supplierDto.PhoneNumber,
            Email = supplierDto.Email,
        };
    }
}