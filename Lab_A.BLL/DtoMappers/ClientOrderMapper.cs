using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.BLL.DtoMappers;

public static class ClientOrderMapper
{
    public static ClientOrderDto ToDto(this IClientOrder clientOrder)
    {
        return new ClientOrderDto()
        {
            ClientOrderId = clientOrder.ClientOrderId,
            Number = (int)clientOrder.Number!,
            Fullprice = (double)clientOrder.Fullprice!,
            ClientId = (int)clientOrder.ClientId!,
            EmployeeId = (int)clientOrder.EmployeeId!,
            StatusId = clientOrder.StatusId,
            BiomaterialCollectionDate = (DateTime)clientOrder.BiomaterialCollectionDate!,
            Client = clientOrder.Client.ToDto(),
            Employee = clientOrder.Employee.ToDto(),
            Status = clientOrder.Status.ToDto(),
            OrderAnalyses = clientOrder.OrderAnalyses.Select(x => x.ToDto()).ToList(),
            BiomaterialCollections = clientOrder.BiomaterialCollections?.Select(x => x.ToDto()).ToList(),
        };
    }

    public static ClientOrderDto ToDto2(this IClientOrder clientOrder)
    {
        return new ClientOrderDto()
        {
            ClientOrderId = clientOrder.ClientOrderId,
            Number = (int)clientOrder.Number!,
            Fullprice = (double)clientOrder.Fullprice!,
            ClientId = (int)clientOrder.ClientId!,
            EmployeeId = (int)clientOrder.EmployeeId!,
            StatusId = clientOrder.StatusId,
            BiomaterialCollectionDate = (DateTime)clientOrder.BiomaterialCollectionDate!,
            Client = clientOrder.Client.ToDto(),
            Employee = clientOrder.Employee.ToDto(),
            Status = clientOrder.Status.ToDto(),
        };
    }

    public static IClientOrder ToEntity(this ClientOrderDto clientOrderDto)
    {
        return new ClientOrder
        {
            ClientOrderId = clientOrderDto.ClientOrderId,
            Number = clientOrderDto.Number,
            Fullprice = clientOrderDto.Fullprice,
            ClientId = clientOrderDto.ClientId,
            EmployeeId = clientOrderDto.EmployeeId,
            StatusId = clientOrderDto.StatusId,
            BiomaterialCollectionDate = clientOrderDto.BiomaterialCollectionDate,
            Client = (Client)clientOrderDto.Client.ToEntity(),
            Employee = (Employee)clientOrderDto.Employee.ToEntity(),
            Status = (Status)clientOrderDto.Status.ToEntity(),
            OrderAnalyses = clientOrderDto.OrderAnalyses!.Select(x => (OrderAnalysis)x.ToEntity()).ToList(),
            BiomaterialCollections = clientOrderDto.BiomaterialCollections!.Select(x => (BiomaterialCollection)x.ToEntity()).ToList(),
        };
    }
}