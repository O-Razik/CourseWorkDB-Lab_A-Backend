using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;
using Microsoft.IdentityModel.Tokens;

namespace Lab_A.BLL.DtoMappers;

public static class LaboratoryMapper
{
    public static LaboratoryDto ToDto(this ILaboratory laboratory)
    {
        return new LaboratoryDto()
        {
            LaboratoryId = laboratory.LaboratoryId,
            CityId = laboratory.CityId,
            Address = laboratory.Address,
            PhoneNumber = laboratory.PhoneNumber,
            City = laboratory.City.ToDto(),
            LaboratorySchedules = laboratory.LaboratorySchedules.Select(x => x.ToDto()).ToList()
        };
    }
    public static ILaboratory ToEntity(this LaboratoryDto laboratoryDto)
    {
        return new Laboratory
        {
            LaboratoryId = laboratoryDto.LaboratoryId,
            CityId = laboratoryDto.CityId,
            Address = laboratoryDto.Address,
            PhoneNumber = laboratoryDto.PhoneNumber,
            City = (City)laboratoryDto.City.ToEntity(),
            LaboratorySchedules = laboratoryDto.LaboratorySchedules!.Select(x => (LaboratorySchedule)x.ToEntity()).ToList()
        };
    }
}