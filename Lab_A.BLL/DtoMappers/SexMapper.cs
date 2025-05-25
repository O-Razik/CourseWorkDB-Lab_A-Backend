using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class SexMapper
{
    public static SexDto ToDto(this ISex sex)
    {
        return new SexDto()
        {
            SexId = sex.SexId,
            SexName = sex.SexName,
        };
    }

    public static ISex ToEntity(this SexDto sexDto)
    {
        return new Sex()
        {
            SexId = sexDto.SexId,
            SexName = sexDto.SexName,
        };
    }
}