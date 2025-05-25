using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly IDayService _dayService;

    public ScheduleController(IScheduleService scheduleService, IDayService dayService)
    {
        _scheduleService = scheduleService;
        _dayService = dayService;
    }

    // api/Schedule

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAllSchedules()
    {
        var result = await _scheduleService.ReadAllAsync();
        return Ok(result.Select(s => s.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
    {
        var result = await _scheduleService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleDto>> CreateSchedule([FromBody] ScheduleDto schedule)
    {
        var result = await _scheduleService.CreateAsync(schedule.ToEntity());
        return CreatedAtAction(nameof(GetSchedule), new { id = result.ScheduleId }, result.ToDto());
    }

    [HttpPut]
    public async Task<ActionResult<ScheduleDto>> UpdateSchedule([FromBody] ScheduleDto schedule)
    {
        var result = await _scheduleService.UpdateAsync(schedule.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteSchedule(int id)
    {
        var result = await _scheduleService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    // api/Schedule/Day

    [HttpGet("Day/all")]
    public async Task<ActionResult<IEnumerable<DayDto>>> GetAllDays()
    {
        var result = await _dayService.ReadAllAsync();
        return Ok(result.Select(d => d.ToDto()));
    }

    [HttpGet("Day/{id:int:min(1)}")]
    public async Task<ActionResult<DayDto>> GetDay(int id)
    {
        var result = await _dayService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}