using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class ClientOrderController : ControllerBase
{
    private readonly IClientOrderService _clientOrderService;
    private readonly IOrderAnalysisService _orderAnalysisService;

    public ClientOrderController(IClientOrderService clientOrderService, IOrderAnalysisService orderAnalysisService)
    {
        _clientOrderService = clientOrderService;
        _orderAnalysisService = orderAnalysisService;
    }

    // api/ClientOrder

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> GetAllClientOrders(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? employeeId,
        [FromQuery] string? clientFullname,
        [FromQuery] double? minPrice,
        [FromQuery] double? maxPrice,
        [FromQuery] List<int>? statusIds,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _clientOrderService.ReadAllAsync(
            fromDate: fromDate,
            toDate: toDate,
            employeeId: employeeId,
            clientFullname: clientFullname,
            minPrice: minPrice,
            maxPrice: maxPrice,
            statusIds: statusIds,
            pageNumber: pageNumber,
            pageSize: pageSize);

        return Ok(result.Select(co => co.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ClientOrderDto>> GetClientOrder(int id)
    {
        var result = await _clientOrderService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpGet("Client/{clientId:int:min(1)}")]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> GetClientOrdersByClientId(int clientId)
    {
        var result = await _clientOrderService.ReadAllByClientIdAsync(clientId);
        return Ok(result.Select(co => co.ToDto()));
    }

    [HttpPost]
    public async Task<ActionResult<ClientOrderDto>> CreateClientOrder([FromBody] ClientOrderDto clientOrderDto)
    {
        var clientOrder = clientOrderDto.ToEntity();
        var result = await _clientOrderService.CreateAsync(clientOrder);
        return CreatedAtAction(nameof(GetClientOrder), new { id = result.ClientOrderId }, result.ToDto());
    }
    [HttpPut]
    public async Task<ActionResult<ClientOrderDto>> UpdateClientOrder([FromBody] ClientOrderDto clientOrderDto)
    {
        var clientOrder = clientOrderDto.ToEntity();
        var result = await _clientOrderService.UpdateAsync(clientOrder);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteClientOrder(int id)
    {
        var result = await _clientOrderService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    // api/ClientOrder/OrderAnalysis

    [HttpGet("{id:int:min(1)}/OrderAnalysis")]
    public async Task<ActionResult<IEnumerable<OrderAnalysisDto>>> GetOrderAnalysesByClientOrderId(int id)
    {
        var result = await _orderAnalysisService.ReadAllByClientOrderIdAsync(id);
        return Ok(result.Select(oa => oa.ToDto()));
    }

    [HttpGet("OrderAnalysis/{id:int:min(1)}")]
    public async Task<ActionResult<OrderAnalysisDto>> GetOrderAnalysis(int id)
    {
        var result = await _orderAnalysisService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpPost("OrderAnalysis")]
    public async Task<ActionResult<OrderAnalysisDto>> CreateOrderAnalysis([FromBody] OrderAnalysisDto orderAnalysis)
    {
        var result = await _orderAnalysisService.CreateAsync(orderAnalysis.ToEntity());
        return CreatedAtAction(nameof(GetOrderAnalysis), new { id = result.OrderAnalysisId }, result.ToDto());
    }
    [HttpPut("OrderAnalysis")]
    public async Task<ActionResult<OrderAnalysisDto>> UpdateOrderAnalysis([FromBody] OrderAnalysisDto orderAnalysis)
    {
        var result = await _orderAnalysisService.UpdateAsync(orderAnalysis.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    [HttpDelete("OrderAnalysis/{id:int:min(1)}")]
    public async Task<ActionResult> DeleteOrderAnalysis(int id)
    {
        var result = await _orderAnalysisService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

}