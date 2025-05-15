using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamsController : ControllerBase
    {
        private readonly ExamService _examService;
        private readonly IResponseHandler _responseHandler;

        public ExamsController(ExamService examService, IResponseHandler responseHandler)
        {
            _examService = examService;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExamDto dto)
        {
            try
            {
                var result = await _examService.CreateExamAsync(dto);
                return _responseHandler.Created(result, "Exam created successfully");
            }
            catch (InvalidOperationException ex)
            {
                return _responseHandler.BadRequest(ex.Message);
            }
        }

        [HttpGet("{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var result = await _examService.GetByStageIdAsync(stageId);
            return result != null
                ? _responseHandler.Success(result)
                : _responseHandler.NotFound("Exam not found");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExamDto dto)
        {
            var result = await _examService.UpdateExamAsync(dto);
            return result != null
                ? _responseHandler.Success(result, "Exam updated successfully")
                : _responseHandler.NotFound("Exam not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _examService.SoftDeleteExamAsync(id);
            return result
                ? _responseHandler.Success(message: "Exam soft-deleted successfully")
                : _responseHandler.NotFound("Exam not found or already inactive");
        }
    }
}
