namespace Lab_A.Abstraction.IModels;

public interface IDay
{
    public int DayId { get; set; }
    
    public string DayName { get; set; }

    public ICollection<ISchedule> Schedules { get; set; }
}