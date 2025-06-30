using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;


[Route("api/[controller]")]
[ApiController]
public class LaboratoryController : ControllerBase
{
    private readonly ILaboratoryService _laboratoryService;
    private readonly IInventoryInLaboratoryService _inventoryInLaboratoryService;
    private readonly ILaboratoryScheduleService _laboratoryScheduleService;

    public LaboratoryController(ILaboratoryService laboratoryService, IInventoryInLaboratoryService inventoryInLaboratoryService, ILaboratoryScheduleService laboratoryScheduleService)
    {
        _laboratoryService = laboratoryService;
        _inventoryInLaboratoryService = inventoryInLaboratoryService;
        _laboratoryScheduleService = laboratoryScheduleService;
    }

    // api/Laboratory

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<LaboratoryDto>>> GetAllLaboratories()
    {
        var result = await _laboratoryService.ReadAllAsync();
        return Ok(result.Select(l => l.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<LaboratoryDto>> GetLaboratory(int id)
    {
        var result = await _laboratoryService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<LaboratoryDto>> CreateLaboratory([FromBody] LaboratoryDto laboratory)
    {
        var result = await _laboratoryService.CreateAsync(laboratory.ToEntity());
        return CreatedAtAction(nameof(GetLaboratory), new { id = result.LaboratoryId }, result.ToDto());
    }

    [HttpPut]
    public async Task<ActionResult<LaboratoryDto>> UpdateLaboratory([FromBody] LaboratoryDto laboratory)
    {
        var result = await _laboratoryService.UpdateAsync(laboratory.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    // api/Laboratory/Inventory

    [HttpGet("{id:int:min(1)}/Inventory/{isZero:bool}")]
    public async Task<ActionResult<IEnumerable<InventoryInLaboratoryDto>>> GetInventoryInLaboratory(int id, bool isZero)
    {
        var result = await _inventoryInLaboratoryService.ReadAllByLaboratoryAsync(id, isZero);
        return Ok(result.Select(i => i.ToDto()));
    }

    [HttpGet("Inventory/{inventoryId:int:min(1)}")]
    public async Task<ActionResult<InventoryInLaboratoryDto>> GetInventory(int inventoryId)
    {
        var result = await _inventoryInLaboratoryService.ReadAsync(inventoryId);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost("Inventory")]
    public async Task<ActionResult<InventoryInLaboratoryDto>> CreateInventoryInLaboratory([FromBody] InventoryInLaboratoryDto inventoryInLaboratoryDto)
    {
        var inventoryInLaboratory = inventoryInLaboratoryDto.ToEntity();
        var result = await _inventoryInLaboratoryService.CreateAsync(inventoryInLaboratory);
        return CreatedAtAction(nameof(GetInventory), new { id = result.InventoryInLaboratoryId }, result.ToDto());
    }

    [HttpPut("Inventory")]
    public async Task<ActionResult<InventoryInLaboratoryDto>> UpdateInventoryInLaboratory([FromBody] InventoryInLaboratoryDto inventoryInLaboratoryDto)
    {
        var inventoryInLaboratory = inventoryInLaboratoryDto.ToEntity();
        var result = await _inventoryInLaboratoryService.UpdateAsync(inventoryInLaboratory);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    // api/Laboratory/LaboratorySchedule

    [HttpGet("{id:int:min(1)}/LaboratorySchedule")]
    public async Task<ActionResult<IEnumerable<LaboratoryScheduleDto>>> GetLaboratoryScheduleForLaboratory(int laboratoryId)
    {
        var result = await _laboratoryScheduleService.ReadByLaboratoryAsync(laboratoryId);
        return Ok(result.Select(s => s.ToDto()));
    }

    [HttpGet("LaboratorySchedule/{scheduleId:int:min(1)}")]
    public async Task<ActionResult<LaboratoryScheduleDto>> GetLaboratorySchedule(int scheduleId)
    {
        var result = await _laboratoryScheduleService.ReadAsync(scheduleId);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost("LaboratorySchedule")]
    public async Task<ActionResult<LaboratoryScheduleDto>> CreateLaboratorySchedule([FromBody] LaboratoryScheduleDto laboratoryScheduleDto)
    {
        var laboratorySchedule = laboratoryScheduleDto.ToEntity();
        var result = await _laboratoryScheduleService.CreateAsync(laboratorySchedule);
        return CreatedAtAction(nameof(GetLaboratorySchedule), new { id = result.ScheduleId }, result.ToDto());
    }

    [HttpPut("LaboratorySchedule")]
    public async Task<ActionResult<LaboratoryScheduleDto>> UpdateLaboratorySchedule([FromBody] LaboratoryScheduleDto laboratoryScheduleDto)
    {
        var laboratorySchedule = laboratoryScheduleDto.ToEntity();
        var result = await _laboratoryScheduleService.UpdateAsync(laboratorySchedule);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}