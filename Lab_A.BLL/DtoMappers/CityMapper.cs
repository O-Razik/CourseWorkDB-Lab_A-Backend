using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class CityMapper
{
    public static CityDto ToDto(this ICity city)
    {
        return new CityDto()
        {
            CityId = city.CityId,
            CityName = city.CityName
        };
    }
    public static ICity ToEntity(this CityDto cityDto)
    {
        return new City
        {
            CityId = cityDto.CityId,
            CityName = cityDto.CityName
        };
    }
}