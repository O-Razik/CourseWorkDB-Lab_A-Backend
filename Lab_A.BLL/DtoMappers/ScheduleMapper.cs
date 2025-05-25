using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class ScheduleMapper
{
    public static ScheduleDto ToDto(this ISchedule schedule)
    {
        return new ScheduleDto()
        {
            ScheduleId = schedule.ScheduleId,
            DayId = schedule.DayId,
            StartTime = schedule.StartTime,
            CollectionEndTime = schedule.CollectionEndTime,
            EndTime = schedule.EndTime,
            Day = schedule.Day.ToDto()
        };
    }
    public static ISchedule ToEntity(this ScheduleDto scheduleDto)
    {
        return new Schedule
        {
            ScheduleId = scheduleDto.ScheduleId,
            DayId = scheduleDto.DayId,
            StartTime = scheduleDto.StartTime,
            CollectionEndTime = scheduleDto.CollectionEndTime,
            EndTime = scheduleDto.EndTime,
            Day = (Day)scheduleDto.Day.ToEntity()
        };
    }
}