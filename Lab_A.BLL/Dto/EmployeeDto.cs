namespace Lab_A.BLL.Dto;

public class EmployeeDto
{
    public int EmployeeId { get; set; }

    public int PositionId { get; set; }

    public int LaboratoryId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public LaboratoryDto? Laboratory { get; set; }

    public PositionDto? Position { get; set; }
}