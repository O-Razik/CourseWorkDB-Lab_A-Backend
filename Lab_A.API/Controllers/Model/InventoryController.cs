using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetAllInventories()
    {
        var result = await _inventoryService.ReadAllAsync();
        return Ok(result.Select(i => i.ToDto()));
    }
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryDto>> GetInventory(int id)
    {
        var result = await _inventoryService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpPost]
    public async Task<ActionResult<InventoryDto>> CreateInventory([FromBody] InventoryDto inventoryDto)
    {
        var inventory = inventoryDto.ToEntity();
        var result = await _inventoryService.CreateAsync(inventory);
        return CreatedAtAction(nameof(GetInventory), new { id = result.InventoryId }, result.ToDto());
    }
    [HttpPut]
    public async Task<ActionResult<InventoryDto>> UpdateInventory([FromBody] InventoryDto inventoryDto)
    {
        var inventory = inventoryDto.ToEntity();
        var result = await _inventoryService.UpdateAsync(inventory);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteInventory(int id)
    {
        var result = await _inventoryService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}