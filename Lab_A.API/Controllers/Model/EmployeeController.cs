using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPositionService _positionService;

    public EmployeeController(IEmployeeService employeeService, IPositionService positionService)
    {
        _employeeService = employeeService;
        _positionService = positionService;
    }
    
    // api/Employee

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllEmployees(
        [FromQuery] string? search,
        [FromQuery] int? laboratoryId,
        [FromQuery] List<int>? positionIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _employeeService.ReadAllAsync(
            search: search,
            laboratoryId: laboratoryId,
            positionIds: positionIds,
            pageNumber: pageNumber,
            pageSize: pageSize);
        return Ok(result.Select(e => e.ToDto()));
    }
    
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var result = await _employeeService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] EmployeeDto employeeDto)
    {
        var employee = employeeDto.ToEntity();
        var result = await _employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetEmployee), new { id = result.EmployeeId }, result.ToDto());
    }
    
    [HttpPut]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee([FromBody] EmployeeDto employeeDto)
    {
        var employee = employeeDto.ToEntity();
        var result = await _employeeService.UpdateAsync(employee);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var result = await _employeeService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    // api/Employee/Position
    [HttpGet("Position/all")]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetAllPositions()
    {
        var result = await _positionService.ReadAllAsync();
        return Ok(result.Select(p => p.ToDto()));
    }

    [HttpGet("Position/{id:int:min(1)}")]
    public async Task<ActionResult<PositionDto>> GetPosition(int id)
    {
        var result = await _positionService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}