using Lab_A.Abstraction.IServices;
using Lab_A.BLL.Dto;
using Lab_A.BLL.DtoMappers;
using Microsoft.AspNetCore.Mvc;

namespace Lab_A.API.Controllers.Model
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalysisResultController : ControllerBase
    {
        private readonly IAnalysisResultService _analysisResultService;

        public AnalysisResultController(IAnalysisResultService analysisResultService)
        {
            _analysisResultService = analysisResultService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<AnalysisResultDto>>> GetAllAnalysisResults(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? analysisCenterId = null,
            [FromQuery] int? analysisId = null,
            [FromQuery] string? clientFullname = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var results = await _analysisResultService.ReadAllAsync(
                fromDate: fromDate,
                toDate: toDate,
                analysisCenterId: analysisCenterId,
                analysisId: analysisId,
                clientFullname: clientFullname,
                pageNumber: pageNumber,
                pageSize: pageSize);

            return Ok(results.Select(ar => ar.ToDto()));
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<AnalysisResultDto>> GetAnalysisResult(int id)
        {
            var result = await _analysisResultService.ReadAsync(id);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<AnalysisResultDto>> CreateAnalysisResult([FromBody] AnalysisResultDto analysisResultDto)
        {
            var analysisResult = analysisResultDto.ToEntity();
            var result = await _analysisResultService.CreateAsync(analysisResult);
            return CreatedAtAction(nameof(GetAnalysisResult), new { id = result.AnalysisResultId }, result.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult<AnalysisResultDto>> UpdateAnalysisResult([FromBody] AnalysisResultDto analysisResultDto)
        {
            var analysisResult = analysisResultDto.ToEntity();
            var result = await _analysisResultService.UpdateAsync(analysisResult);
            return result == null ? NotFound() : Ok(result.ToDto());
        }

        [HttpGet("{id}/pdf")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DownloadPdfReport(int id)
        {
            try
            {
                var pdfStream = await _analysisResultService.GeneratePdfReportAsync(id);
                return File(pdfStream, "application/pdf", $"analysis_result_{id}.pdf");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Сталася помилка при генерації PDF звіту");
            }
        }
    }
}
