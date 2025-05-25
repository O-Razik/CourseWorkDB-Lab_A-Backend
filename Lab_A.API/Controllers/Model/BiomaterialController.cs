using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiomaterialController : ControllerBase
    {
        private readonly IBiomaterialService _biomaterialService;

        public BiomaterialController(IBiomaterialService biomaterialService)
        {
            _biomaterialService = biomaterialService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<BiomaterialDto>>> GetAllBiomaterials()
        {
            var result = await _biomaterialService.ReadAllAsync();
            return Ok(result.Select(b => b.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<BiomaterialDto>> GetBiomaterial(int id)
        {
            var result = await _biomaterialService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<BiomaterialDto>> CreateBiomaterial([FromBody] BiomaterialDto biomaterialDto)
        {
            var biomaterial = biomaterialDto.ToEntity();
            var result = await _biomaterialService.CreateAsync(biomaterial);
            return CreatedAtAction(nameof(GetBiomaterial), new { id = result.BiomaterialId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<BiomaterialDto>> UpdateBiomaterial([FromBody] BiomaterialDto biomaterialDto)
        {
            var biomaterial = biomaterialDto.ToEntity();
            var result = await _biomaterialService.UpdateAsync(biomaterial);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteBiomaterial(int id)
        {
            var result = await _biomaterialService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
