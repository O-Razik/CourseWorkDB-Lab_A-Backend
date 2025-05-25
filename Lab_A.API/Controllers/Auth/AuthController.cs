using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Auth;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService<UserDto> _authService;

    public AuthController(IAuthService<UserDto> authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto?>> Login([FromBody] AuthDto user)
    {
        var userToken = await _authService.LoginAsync(user.Email, user.Password);
        if (userToken == null)
        {
            return Unauthorized();
        }
        return Ok(userToken);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto?>> Register([FromBody] AuthDto user)
    {
        if (string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Password cannot be null or empty.");
        }

        try
        {
            var existingUser = await _authService.LoginAsync(user.Email, user.Password);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error checking user existence: {ex.Message}");
        }

        var userToken = await _authService.RegisterAsync(user.Email, user.Password);
        return CreatedAtAction(nameof(Login), new { email = user.Email }, userToken);
    }

}