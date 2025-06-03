using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiomaterialCollectionController : ControllerBase
    {
        private readonly IBiomaterialCollectionService _biomaterialCollectionService;

        public BiomaterialCollectionController(IBiomaterialCollectionService biomaterialCollectionService)
        {
            _biomaterialCollectionService = biomaterialCollectionService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BiomaterialCollectionDto>>> GetAllBiomaterialCollections(
            [FromQuery] DateTime? fromExpirationDate = null,
            [FromQuery] DateTime? toExpirationDate = null,
            [FromQuery] DateTime? fromCollectionDate = null,
            [FromQuery] DateTime? toCollectionDate = null,
            [FromQuery] int? laboratoryId = null,
            [FromQuery] int? inventoryId = null,
            [FromQuery] int? biomaterialId = null,
            [FromQuery] string? search = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool notDelivered = false
            )
        {
            var result = await _biomaterialCollectionService.ReadAllAsync(
                fromCollectionDate: fromCollectionDate,
                toCollectionDate: toCollectionDate,
                fromExpirationDate: fromExpirationDate,
                toExpirationDate: toExpirationDate,
                laboratoryId: laboratoryId,
                inventoryId: inventoryId,
                biomaterialId: biomaterialId,
                pageNumber: pageNumber,
                pageSize: pageSize,
                notDelivered: notDelivered

            );
            return Ok(result.Select(bc => bc.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<BiomaterialCollectionDto>> GetBiomaterialCollection(int id)
        {
            var result = await _biomaterialCollectionService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<BiomaterialCollectionDto>> CreateBiomaterialCollection([FromBody] BiomaterialCollectionDto biomaterialCollectionDto)
        {
            var result = await _biomaterialCollectionService.CreateAsync(biomaterialCollectionDto.ToEntity());
            return CreatedAtAction(nameof(GetBiomaterialCollection), new { id = result.BiomaterialCollectionId }, result);
        }

        [HttpPut]
        public async Task<ActionResult<BiomaterialCollectionDto>> UpdateBiomaterialCollection([FromBody] BiomaterialCollectionDto biomaterialCollectionDto)
        {
            var result = await _biomaterialCollectionService.UpdateAsync(biomaterialCollectionDto.ToEntity());
            return result == null ? NotFound() : Ok(result.ToDto());
        }
    }
}
