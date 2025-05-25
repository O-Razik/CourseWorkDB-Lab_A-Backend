using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class LaboratoryScheduleMapper
{
    public static LaboratoryScheduleDto ToDto(this ILaboratorySchedule laboratorySchedule)
    {
        return new LaboratoryScheduleDto()
        {
            LaboratoryScheduleId = laboratorySchedule.LaboratoryScheduleId,
            LaboratoryId = laboratorySchedule.LaboratoryId,
            ScheduleId = laboratorySchedule.ScheduleId,
            Schedule = laboratorySchedule.Schedule.ToDto()
        };
    }
    public static ILaboratorySchedule ToEntity(this LaboratoryScheduleDto laboratoryScheduleDto)
    {
        return new LaboratorySchedule
        {
            LaboratoryScheduleId = laboratoryScheduleDto.LaboratoryScheduleId,
            LaboratoryId = laboratoryScheduleDto.LaboratoryId,
            ScheduleId = laboratoryScheduleDto.ScheduleId,
            Schedule = (Schedule)laboratoryScheduleDto.Schedule.ToEntity()
        };
    }
}