using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        private readonly IAnalysisBiomaterialService _analysisBiomaterialService;

        public AnalysisController(IAnalysisService analysisService, IAnalysisBiomaterialService analysisBiomaterialService)
        {
            _analysisService = analysisService;
            _analysisBiomaterialService = analysisBiomaterialService;
        }

        // api/Analysis

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AnalysisDto>>> GetAllAnalyses()
        {
            var result = await _analysisService.ReadAllAsync();
            return Ok(result.Select(a => a.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AnalysisDto>> GetAnalysis(int id)
        {
            var result = await _analysisService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<AnalysisDto>> CreateAnalysis([FromBody] AnalysisDto analysisDto)
        {
            var analysis = analysisDto.ToEntity();
            var result = await _analysisService.CreateAsync(analysis);
            return CreatedAtAction(nameof(GetAnalysis), new { id = result.AnalysisId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<AnalysisDto>> UpdateAnalysis([FromBody] AnalysisDto analysisDto)
        {
            var analysis = analysisDto.ToEntity();
            var result = await _analysisService.UpdateAsync(analysis);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAnalysis(int id)
        {
            var result = await _analysisService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }

        // api/Analysis/AnalysisBiomaterial

        [HttpGet("{id:int:min(1)}/AnalysisBiomaterial")]
        public async Task<ActionResult<IEnumerable<AnalysisBiomaterialDto>>> GetAnalysisBiomaterialsByAnalysisId(int id)
        {
            var result = await _analysisBiomaterialService.ReadAllByAnalysisIdAsync(id);
            return Ok(result.Select(ab => ab.ToDto()));
        }

        [HttpGet("AnalysisBiomaterial/{id:int:min(1)}")]
        public async Task<ActionResult<AnalysisBiomaterialDto>> GetAnalysisBiomaterial(int id)
        {
            var result = await _analysisBiomaterialService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost("AnalysisBiomaterial")]
        public async Task<ActionResult<AnalysisBiomaterialDto>> CreateAnalysisBiomaterial([FromBody] AnalysisBiomaterialDto analysisBiomaterialDto)
        {
            var analysisBiomaterial = analysisBiomaterialDto.ToEntity();
            var result = await _analysisBiomaterialService.CreateAsync(analysisBiomaterial);
            return CreatedAtAction(nameof(GetAnalysisBiomaterial), new { id = result.AnalysisBiomaterialId }, result.ToDto());
        }

        [HttpPut("AnalysisBiomaterial")]
        public async Task<ActionResult<AnalysisBiomaterialDto>> UpdateAnalysisBiomaterial([FromBody] AnalysisBiomaterialDto analysisBiomaterialDto)
        {
            var analysisBiomaterial = analysisBiomaterialDto.ToEntity();
            var result = await _analysisBiomaterialService.UpdateAsync(analysisBiomaterial);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpDelete("AnalysisBiomaterial/{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAnalysisBiomaterial(int id)
        {
            var result = await _analysisBiomaterialService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
