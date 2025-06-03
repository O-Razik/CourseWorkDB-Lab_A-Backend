using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisCenterController : ControllerBase
    {
        private readonly IAnalysisCenterService _analysisCenterService;

        public AnalysisCenterController(IAnalysisCenterService analysisCenterService)
        {
            _analysisCenterService = analysisCenterService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AnalysisCenterDto>>> GetAllAnalysisCenters()
        {
            var result = await _analysisCenterService.ReadAllAsync();
            return Ok(result.Select(ac => ac.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AnalysisCenterDto>> GetAnalysisCenter(int id)
        {
            var result = await _analysisCenterService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<AnalysisCenterDto>> CreateAnalysisCenter([FromBody] AnalysisCenterDto analysisCenterDto)
        {
            var analysisCenter = analysisCenterDto.ToEntity();
            var result = await _analysisCenterService.CreateAsync(analysisCenter);
            return CreatedAtAction(nameof(GetAnalysisCenter), new { id = result.AnalysisCenterId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<AnalysisCenterDto>> UpdateAnalysisCenter([FromBody] AnalysisCenterDto analysisCenterDto)
        {
            var analysisCenter = analysisCenterDto.ToEntity();
            var result = await _analysisCenterService.UpdateAsync(analysisCenter);
            return result == null ? NotFound() : Ok(result.ToDto());
        }
    }
}
