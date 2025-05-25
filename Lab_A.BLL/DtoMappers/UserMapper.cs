using Lab_A.Abstraction.IModels;
using Lab_A.BLL.Dto;
using Lab_A.DAL.Models;

namespace Lab_A.BLL.DtoMappers;

public static class UserMapper
{
    public static UserDto ToDto(this IUserEmployee user, string token)
    {
        return new UserDto
        {
            Employee = (user.Employee).ToDto(),
            Token = token,
        };
    }

    public static IUserEmployee ToModel(this UserDto userDto)
    {
        return new UserEmployee
        {
            Employee = (Employee)userDto.Employee.ToEntity(),
        };
    }
}