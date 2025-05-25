using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model;

[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetAllSuppliers()
    {
        var result = await _supplierService.ReadAllAsync();
        return Ok(result.Select(s => s.ToDto()));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
    {
        var result = await _supplierService.ReadAsync(id);
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] SupplierDto supplier)
    {
        var result = await _supplierService.CreateAsync(supplier.ToEntity());
        return CreatedAtAction(nameof(GetSupplier), new { id = result.SupplierId }, result.ToDto());
    }

    [HttpPut]
    public async Task<ActionResult<SupplierDto>> UpdateSupplier([FromBody] SupplierDto supplier)
    {
        var result = await _supplierService.UpdateAsync(supplier.ToEntity());
        return result == null ? NotFound() : Ok(result.ToDto());
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult> DeleteSupplier(int id)
    {
        var result = await _supplierService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}