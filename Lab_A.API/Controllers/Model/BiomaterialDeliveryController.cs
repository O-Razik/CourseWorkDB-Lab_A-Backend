using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiomaterialDeliveryController : ControllerBase
    {
        private readonly IBiomaterialDeliveryService _biomaterialDeliveryService;

        public BiomaterialDeliveryController(IBiomaterialDeliveryService biomaterialDeliveryService)
        {
            _biomaterialDeliveryService = biomaterialDeliveryService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BiomaterialDeliveryDto>>> GetAllBiomaterialDeliveries(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? analysisCenterId = null,
            [FromQuery] int? statusId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _biomaterialDeliveryService.ReadAllAsync(
                fromDate: fromDate,
                toDate: toDate,
                analysisCenterId: analysisCenterId,
                statusId: statusId,
                pageNumber: pageNumber,
                pageSize: pageSize
            );
            return Ok(result.Select(bc => bc.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<BiomaterialDeliveryDto>> GetBiomaterialDelivery(int id)
        {
            var result = await _biomaterialDeliveryService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<BiomaterialDeliveryDto>> CreateBiomaterialDelivery([FromBody] BiomaterialDeliveryDto biomaterialDeliveryDto)
        {
            var result = await _biomaterialDeliveryService.CreateAsync(biomaterialDeliveryDto.ToEntity());
            return CreatedAtAction(nameof(GetBiomaterialDelivery), new { id = result.BiomaterialDeliveryId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<BiomaterialDeliveryDto>> UpdateBiomaterialDelivery([FromBody] BiomaterialDeliveryDto biomaterialDeliveryDto)
        {
            var result = await _biomaterialDeliveryService.UpdateAsync(biomaterialDeliveryDto.ToEntity());
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteBiomaterialDelivery(int id)
        {
            var result = await _biomaterialDeliveryService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
