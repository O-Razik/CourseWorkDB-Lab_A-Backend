using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class StatusMapper
{
    public static StatusDto ToDto(this IStatus status)
    {
        return new StatusDto()
        {
            StatusId = status.StatusId,
            StatusName = status.StatusName
        };
    }
    public static IStatus ToEntity(this StatusDto statusDto)
    {
        return new Status
        {
            StatusId = statusDto.StatusId,
            StatusName = statusDto.StatusName
        };
    }
}