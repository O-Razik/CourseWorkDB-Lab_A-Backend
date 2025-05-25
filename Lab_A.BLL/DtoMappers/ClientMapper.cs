using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.BLL.DtoMappers;

public static class ClientMapper
{
    public static ClientDto ToDto(this IClient client)
    {
        return new ClientDto()
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

    public static IClient ToEntity(this ClientDto? clientDto)
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