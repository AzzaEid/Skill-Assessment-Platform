using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.ExamReques;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/exam-request")]
    public class ExamRequestsController : ControllerBase
    {
        private readonly ExamRequestService _examRequestService;
        private readonly IResponseHandler _responseHandler;

        public ExamRequestsController(
            ExamRequestService examRequestService,
            IResponseHandler responseHandler)
        {
            _examRequestService = examRequestService;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamRequestCreateDTO dto)
        {
            var result = await _examRequestService.CreateExamRequestAsync(dto);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _examRequestService.GetExamRequestByIdAsync(id);
            return _responseHandler.Success(result);
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicant(string applicantId)
        {
            var result = await _examRequestService.GetExamRequestsByApplicantIdAsync(applicantId);
            return _responseHandler.Success(result);
        }

        [HttpGet("stage/{stageId}")]
        public async Task<IActionResult> GetByStage(int stageId)
        {
            var result = await _examRequestService.GetExamRequestsByStageIdAsync(stageId);
            return _responseHandler.Success(result);
        }

        [HttpGet("stage/{stageId}/pending")]
        public async Task<IActionResult> GetPendingByStage(int stageId)
        {
            var result = await _examRequestService.GetPendingExamRequestsByStageIdAsync(stageId);
            return _responseHandler.Success(result);
        }

        [HttpGet("track/{trackId}/pending-summary")]
        public async Task<IActionResult> GetPendingSummary(string trackId)
        {
            var result = await _examRequestService.GetPendingExamRequestsSummaryAsync(trackId);
            return _responseHandler.Success(result);
        }

        [HttpPut("{requestId}/approve")]
        public async Task<IActionResult> Approve(int requestId, [FromBody] ExamRequestUpdateDTO dto)
        {
            var result = await _examRequestService.ApproveExamRequestAsync(requestId, dto);
            return _responseHandler.Success(result);
        }

        [HttpPut("{requestId}/reject")]
        public async Task<IActionResult> Reject(int requestId, [FromQuery] string? message = null)
        {
            var result = await _examRequestService.RejectExamRequestAsync(requestId, message);
            return _responseHandler.Success(result);
        }

        [HttpPut("bulk-update")]
        public async Task<IActionResult> BulkUpdate([FromBody] ExamRequestBulkUpdateDTO dto)
        {
            var result = await _examRequestService.BulkUpdateExamRequestsAsync(dto);
            return _responseHandler.Success(result);
        }
    }
}
