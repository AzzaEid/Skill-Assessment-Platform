using Microsoft.AspNetCore.Http;
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
            private readonly TrackService _trackService;
            private readonly IResponseHandler _responseHandler;
            private readonly ExamService _examService;

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
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var result = await _examService.GetByStageIdAsync(stageId);
            if (result == null)
                return NotFound(new { message = "Exam not found" });

            return _responseHandler.Success(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateExamDto dto)
        {
            var result = await _examService.UpdateExamAsync(dto);
            if (result == null)
                return NotFound(new { message = "Exam not found" });

            return _responseHandler.Success(result, "Exam updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _examService.SoftDeleteExamAsync(id);
            if (!result)
                return NotFound(new { message = "Exam not found or already inactive" });

            return _responseHandler.Success(message: "Exam soft-deleted successfully");
        }



    }

}

