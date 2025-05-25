using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ISexService _sexService;

    public ClientController(IClientService clientService, ISexService sexService)
    {
        _clientService = clientService;
        _sexService = sexService;
    }
    
    // api/Client

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetAllClients(
        [FromQuery] int? sexId,
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var result = await _clientService.ReadAllAsync(
            sexId: sexId,
            search: search,
            pageNumber: pageNumber,
            pageSize: pageSize);
        return Ok(result.Select(c => c.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        var result = await _clientService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient([FromBody] ClientDto clientDto)
    {
        var client = clientDto.ToEntity();
        var result = await _clientService.CreateAsync(client);
        return CreatedAtAction(nameof(GetClient), new { id = result.ClientId }, result.ToDto());
    }
    
    [HttpPut]
    public async Task<ActionResult<ClientDto>> UpdateClient([FromBody] ClientDto clientDto)
    {
        var client = clientDto.ToEntity();
        var result = await _clientService.UpdateAsync(client);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
    
    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var result = await _clientService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    // api/Client/Sex

    [HttpGet("Sex/all")]
    public async Task<ActionResult<IEnumerable<SexDto>>> GetAllSexes()
    {
        var result = await _sexService.ReadAllAsync();
        return Ok(result.Select(s => s.ToDto()));
    }

    [HttpGet("Sex/{id:int:min(1)}")]
    public async Task<ActionResult<SexDto>> GetSex(int id)
    {
        var result = await _sexService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }
}