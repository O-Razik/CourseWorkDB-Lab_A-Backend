using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class PositionMapper
{
    public static PositionDto ToDto(this IPosition position)
    {
        return new PositionDto()
        {
            PositionId = position.PositionId,
            PositionName = position.PositionName,
        };
    }
    public static IPosition ToEntity(this PositionDto positionDto)
    {
        return new Position()
        {
            PositionId = positionDto.PositionId,
            PositionName = positionDto.PositionName,
        };
    }
}