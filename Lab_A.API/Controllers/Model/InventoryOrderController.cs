using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class InventoryOrderController : ControllerBase
{
    private readonly IInventoryOrderService _inventoryOrderService;
    private readonly IInventoryInOrderService _inventoryInOrderService;

    public InventoryOrderController(IInventoryOrderService inventoryOrderService, IInventoryInOrderService inventoryInOrderService)
    {
        _inventoryOrderService = inventoryOrderService;
        _inventoryInOrderService = inventoryInOrderService;
    }

    // api/InventoryOrder

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<InventoryOrderDto>>> GetAllInventoryOrders(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int? supplierId = null,
        [FromQuery] double? minPrice = null,
        [FromQuery] double? maxPrice = null,
        [FromQuery] IEnumerable<int>? statusIds = null,
        [FromQuery] string? search = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _inventoryOrderService.ReadAllAsync(
            fromDate: fromDate,
            toDate: toDate,
            supplierId: supplierId,
            minPrice: minPrice,
            maxPrice: maxPrice,
            statusIds: statusIds,
            search: search,
            pageNumber: pageNumber,
            pageSize: pageSize
        );
        return Ok(result.Select(io => io.ToDto()));
    }
    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<InventoryOrderDto>> GetInventoryOrder(int id)
    {
        var result = await _inventoryOrderService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpPost]
    public async Task<ActionResult<InventoryOrderDto>> CreateInventoryOrder([FromBody] InventoryOrderDto inventoryOrder)
    {
        var result = await _inventoryOrderService.CreateAsync(inventoryOrder.ToEntity());
        return CreatedAtAction(nameof(GetInventoryOrder), new { id = result.InventoryOrderId }, result.ToDto());
    }
    [HttpPut]
    public async Task<ActionResult<InventoryOrderDto>> UpdateInventoryOrder([FromBody] InventoryOrderDto inventoryOrder)
    {
        var result = await _inventoryOrderService.UpdateAsync(inventoryOrder.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    
    [HttpPatch("{id:int:min(1)}/cancel")]
    public async Task<ActionResult<InventoryOrderDto>> CancelInventoryOrder([FromRoute] int id)
    {
        var result = await _inventoryOrderService.CancelOrderAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    // api/InventoryOrder/InventoryInOrder

    [HttpGet("{id:int:min(1)}/InventoryInOrder")]
    public async Task<ActionResult<IEnumerable<InventoryInOrderDto>>> GetInventoryInOrdersByInventoryOrderId(int id)
    {
        var result = await _inventoryInOrderService.ReadAllByInventoryOrderIdAsync(id);
        return Ok(result.Select(io => io.ToDto()));
    }

    [HttpGet("InventoryInOrder/{id:int:min(1)}")]
    public async Task<ActionResult<InventoryInOrderDto>> GetInventoryInOrder(int id)
    {
        var result = await _inventoryInOrderService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost("InventoryInOrder")]
    public async Task<ActionResult<InventoryInOrderDto>> CreateInventoryInOrder([FromBody] InventoryInOrderDto inventoryInOrderDto)
    {
        var inventoryInOrder = inventoryInOrderDto.ToEntity();
        var result = await _inventoryInOrderService.CreateAsync(inventoryInOrder);
        return CreatedAtAction(nameof(GetInventoryInOrder), new { id = result.InventoryInOrderId }, result.ToDto());
    }

    [HttpPut("InventoryInOrder")]
    public async Task<ActionResult<InventoryInOrderDto>> UpdateInventoryInOrder([FromBody] InventoryInOrderDto inventoryInOrderDto)
    {
        var inventoryInOrder = inventoryInOrderDto.ToEntity();
        var result = await _inventoryInOrderService.UpdateAsync(inventoryInOrder);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}