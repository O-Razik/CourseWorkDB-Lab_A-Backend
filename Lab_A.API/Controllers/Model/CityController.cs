using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetAllCities()
    {
        var result = await _cityService.ReadAllAsync();
        return Ok(result.Select(c => c.ToDto()));
    }
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<CityDto>> GetCity(int id)
    {
        var result = await _cityService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCity([FromBody] CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        var result = await _cityService.CreateAsync(city);
        return CreatedAtAction(nameof(GetCity), new { id = result.CityId }, result.ToDto());
    }
    [HttpPut]
    public async Task<ActionResult<CityDto>> UpdateCity([FromBody] CityDto cityDto)
    {
        var city = cityDto.ToEntity();
        var result = await _cityService.UpdateAsync(city);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}