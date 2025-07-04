﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.Abstraction.IModels;

[Route("api/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;
    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<StatusDto>>> GetAllStatuses()
    {
        var result = await _statusService.ReadAllAsync();
        return Ok(result.Select(s => s.ToDto()));
    }
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<StatusDto>> GetStatus(int id)
    {
        var result = await _statusService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpPost]
    public async Task<ActionResult<StatusDto>> CreateStatus([FromBody] StatusDto status)
    {
        var result = await _statusService.CreateAsync(status.ToEntity());
        return CreatedAtAction(nameof(GetStatus), new { id = result.StatusId }, result.ToDto());
    }
    [HttpPut]
    public async Task<ActionResult<StatusDto>> UpdateStatus([FromBody] StatusDto status)
    {
        var result = await _statusService.UpdateAsync(status.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}