using Lab_A.Abstraction.IModels;
using Lab_A.Abstraction.IRepositories;
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Lab_A.BLL.Services;

public class AuthService : IAuthService<UserDto>
{
    private readonly IUserEmployeeRepository _userEmployeeRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserEmployeeRepository userEmployeeRepository, IConfiguration configuration)
    {
        _userEmployeeRepository = userEmployeeRepository;
        _configuration = configuration;
    }

    public async Task<UserDto> RegisterAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

        var userModel = await _userEmployeeRepository.RegisterAsync(email, password);

        var createdUser = await _userEmployeeRepository.LoginAsync(email, password);
        if (createdUser == null)
            throw new Exception("User creation failed.");

        var token = GenerateJwtToken(createdUser);
        return userModel.ToDto(token);
    }

    public async Task<UserDto?> LoginAsync(string email, string password)
    {
        var userModel = await _userEmployeeRepository.LoginAsync(email, password);
        if (userModel == null)
            return null;

        var token = GenerateJwtToken(userModel);
        return userModel.ToDto(token);
    }

    public string GenerateJwtToken(IUserEmployee user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "JWT key is not configured.");
        }
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserEmployeeId.ToString()),
                new Claim(ClaimTypes.Role, user.Employee.Position.PositionName)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}