using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class InventoryDeliveryController : ControllerBase
{
    private readonly IInventoryDeliveryService _inventoryDeliveryService;

    public InventoryDeliveryController(IInventoryDeliveryService inventoryDeliveryService)
    {
        _inventoryDeliveryService = inventoryDeliveryService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<InventoryDeliveryDto>>> GetAllInventoryDeliveries(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int? laboratoryId = null,
        [FromQuery] IEnumerable<int>? inventoryIds = null,
        [FromQuery] IEnumerable<int>? statusIds = null,
        [FromQuery] string? search = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
        )
    {
        var result = await _inventoryDeliveryService.ReadAllAsync(
            fromDate: fromDate,
            toDate: toDate,
            laboratoryId: laboratoryId,
            inventoryIds: inventoryIds,
            statusIds: statusIds,
            search: search,
            pageNumber: pageNumber,
            pageSize: pageSize
        );
        return Ok(result.Select(i => i.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryDeliveryDto>> GetInventoryDelivery(int id)
    {
        var result = await _inventoryDeliveryService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<InventoryDeliveryDto>> CreateInventoryDelivery([FromBody] InventoryDeliveryDto inventoryDeliveryDto)
    {
        var inventoryDelivery = inventoryDeliveryDto.ToEntity();
        var result = await _inventoryDeliveryService.CreateAsync(inventoryDelivery);
        return CreatedAtAction(nameof(GetInventoryDelivery), new { id = result.InventoryDeliveryId }, result.ToDto());
    }

    [HttpPut]
    public async Task<ActionResult<InventoryDeliveryDto>> UpdateInventoryDelivery([FromBody] InventoryDeliveryDto inventoryDeliveryDto)
    {
        var inventoryDelivery = inventoryDeliveryDto.ToEntity();
        var result = await _inventoryDeliveryService.UpdateAsync(inventoryDelivery);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    
    [HttpPatch("{deliveryId:int:min(1)}/status/{status:int:min(1)}")]
    public async Task<ActionResult<InventoryDeliveryDto>> UpdateInventoryDeliveryStatus(int deliveryId, int status)
    {
        var result = await _inventoryDeliveryService.UpdateStatusAsync(deliveryId, status);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}