using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisCategoryController : ControllerBase
    {
        private readonly IAnalysisCategoryService _analysisCategoryService;

        public AnalysisCategoryController(IAnalysisCategoryService analysisCategoryService)
        {
            _analysisCategoryService = analysisCategoryService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AnalysisCategoryDto>>> GetAllAnalysisCategories()
        {
            var result = await _analysisCategoryService.ReadAllAsync();
            return Ok(result.Select(ac => ac.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AnalysisCategoryDto>> GetAnalysisCategory(int id)
        {
            var result = await _analysisCategoryService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<AnalysisCategoryDto>> CreateAnalysisCategory([FromBody] AnalysisCategoryDto analysisCategoryDto)
        {
            var analysisCategory = analysisCategoryDto.ToEntity();
            var result = await _analysisCategoryService.CreateAsync(analysisCategory);
            return CreatedAtAction(nameof(GetAnalysisCategory), new { id = result.AnalysisCategoryId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<AnalysisCategoryDto>> UpdateAnalysisCategory([FromBody] AnalysisCategoryDto analysisCategoryDto)
        {
            var analysisCategory = analysisCategoryDto.ToEntity();
            var result = await _analysisCategoryService.UpdateAsync(analysisCategory);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAnalysisCategory(int id)
        {
            var result = await _analysisCategoryService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
