using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewsController : ControllerBase
    {
        private readonly InterviewService _interviewService;
        private readonly IResponseHandler _responseHandler;

        public InterviewsController(InterviewService interviewService, IResponseHandler responseHandler)
        {
            _interviewService = interviewService;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInterviewDto dto)
        {
            var result = await _interviewService.CreateInterviewAsync(dto);
            return _responseHandler.Created(result, "Interview created successfully");
        }

        [HttpGet("{stageId}")]
        public async Task<IActionResult> GetByStageId(int stageId)
        {
            var result = await _interviewService.GetByStageIdAsync(stageId);
            return result != null
                ? _responseHandler.Success(result)
                : _responseHandler.NotFound("Interview not found");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InterviewDto dto)
        {
            var result = await _interviewService.UpdateInterviewAsync(dto);
            return result != null
                ? _responseHandler.Success(result, "Interview updated successfully")
                : _responseHandler.NotFound("Interview not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var result = await _interviewService.SoftDeleteAsync(id);
            return result
                ? _responseHandler.Success(message: "Interview soft-deleted successfully")
                : _responseHandler.NotFound("Interview not found or already inactive");
        }
    }
}
