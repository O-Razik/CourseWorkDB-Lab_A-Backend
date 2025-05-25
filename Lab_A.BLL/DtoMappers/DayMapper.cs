using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class DayMapper
{
    public static DayDto ToDto(this IDay day)
    {
        return new DayDto()
        {
            DayId = day.DayId,
            DayName = day.DayName
        };
    }
    public static IDay ToEntity(this DayDto dayDto)
    {
        return new Day
        {
            DayId = dayDto.DayId,
            DayName = dayDto.DayName,
        };
    }
}