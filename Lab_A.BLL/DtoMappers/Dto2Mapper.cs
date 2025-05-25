/*

using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class Dto2Mapper
{
    public static OrderAnalysisDto2 ToDto2(this IOrderAnalysis orderAnalysis)
    {
        return new OrderAnalysisDto2()
        {
            OrderAnalysisId = orderAnalysis.OrderAnalysisId,
            ClientOrderId = orderAnalysis.ClientOrderId,
            AnalysisId = orderAnalysis.AnalysisId,
            Analysis = orderAnalysis.Analysis.ToDto(),
            ClientOrder = orderAnalysis.ClientOrder.ToDto2(),
        };
    }
    public static IOrderAnalysis ToEntity(this OrderAnalysisDto2 orderAnalysisDto)
    {
        return new OrderAnalysis()
        {
            OrderAnalysisId = orderAnalysisDto.OrderAnalysisId,
            ClientOrderId = orderAnalysisDto.ClientOrderId,
            AnalysisId = orderAnalysisDto.AnalysisId,
            Analysis = (Analysis)orderAnalysisDto.Analysis.ToEntity(),
            ClientOrder = (ClientOrder)orderAnalysisDto.ClientOrder.ToEntity(),
        };
    }

    public static ClientOrderDto2 ToDto2(this IClientOrder clientOrder)
    {
        return new ClientOrderDto2()
        {
            ClientOrderId = clientOrder.ClientOrderId,
            Number = clientOrder.Number,
            Fullprice = clientOrder.Fullprice,
            ClientId = clientOrder.ClientId,
            EmployeeId = clientOrder.EmployeeId,
            BiomaterialCollectionDate = clientOrder.BiomaterialCollectionDate,
            Client = clientOrder.Client.ToDto2(),
            Employee = clientOrder.Employee.ToDto(),
        };
    }

    public static IClientOrder ToEntity(this ClientOrderDto2 clientOrderDto)
    {
        return new ClientOrder
        {
            ClientOrderId = clientOrderDto.ClientOrderId,
            Number = clientOrderDto.Number,
            Fullprice = clientOrderDto.Fullprice,
            ClientId = clientOrderDto.ClientId,
            EmployeeId = clientOrderDto.EmployeeId,
            BiomaterialCollectionDate = clientOrderDto.BiomaterialCollectionDate,
            Client = (Client)clientOrderDto.Client.ToEntity(),
            Employee = (Employee)clientOrderDto.Employee.ToEntity(),
        };
    }

    public static ClientDto2 ToDto2(this IClient client)
    {
        return new ClientDto2()
        {
            ClientId = client.ClientId,
            FirstName = client.FirstName,
            LastName = client.LastName,
            SexId = client.SexId,
            Birthdate = client.Birthdate,
            PhoneNumber = client.PhoneNumber,
            Email = client.Email,
            Sex = client.Sex.ToDto(),
        };
    }
    public static IClient ToEntity(this ClientDto2 clientDto)
    {
        return new Client
        {
            ClientId = clientDto.ClientId,
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Birthdate = clientDto.Birthdate,
            SexId = clientDto.SexId,
            PhoneNumber = clientDto.PhoneNumber,
            Email = clientDto.Email,
            Sex = (Sex)clientDto.Sex.ToEntity(),
        };
    }
}

*/