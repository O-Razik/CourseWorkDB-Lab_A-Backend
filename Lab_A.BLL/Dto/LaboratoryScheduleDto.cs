#nullable disable

namespace Lab_A.BLL.Dto;

public class LaboratoryScheduleDto
{
    public int LaboratoryScheduleId { get; set; }

    public int? LaboratoryId { get; set; }
    
    public int? ScheduleId { get; set; }

    public ScheduleDto Schedule { get; set; }
}