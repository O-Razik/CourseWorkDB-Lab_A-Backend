namespace Lab_A.Abstraction.IModels;

public interface ISchedule
{
    public int ScheduleId { get; set; }
    
    public int DayId { get; set; }
    
    public TimeOnly? StartTime { get; set; }
    
    public TimeOnly? EndTime { get; set; }
    
    public TimeOnly? CollectionEndTime { get; set; }

    public IDay Day { get; set; }

    public ICollection<ILaboratorySchedule> LaboratorySchedules { get; set; }
}