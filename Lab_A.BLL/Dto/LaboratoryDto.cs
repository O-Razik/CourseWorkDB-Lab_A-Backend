namespace Lab_A.BLL.Dto;

public class LaboratoryDto
{
    public int LaboratoryId { get; set; }

    public string Address { get; set; } = null!;

    public int CityId { get; set; }
    
    public string PhoneNumber { get; set; } = null!;
    
    public CityDto City { get; set; } = null!;
    
    public ICollection<LaboratoryScheduleDto>? LaboratorySchedules { get; set; } = new List<LaboratoryScheduleDto>();
}