using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class EmployeeMapper
{
    public static EmployeeDto ToDto(this IEmployee employee)
    {
        return new EmployeeDto()
        {
            EmployeeId = employee.EmployeeId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            PositionId = employee.PositionId,
            LaboratoryId = (int)employee.LaboratoryId!,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            Position = employee.Position.ToDto(),
            Laboratory = employee.Laboratory!.ToDto(),
        };
    }
    public static IEmployee ToEntity(this EmployeeDto employeeDto)
    {
        return new Employee()
        {
            EmployeeId = employeeDto.EmployeeId,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            PositionId = employeeDto.PositionId,
            LaboratoryId = employeeDto.LaboratoryId,
            PhoneNumber = employeeDto.PhoneNumber,
            Email = employeeDto.Email,
            Position = (Position)employeeDto.Position!.ToEntity(),
            Laboratory = (Laboratory)employeeDto.Laboratory!.ToEntity(),
        };
    }
    
}