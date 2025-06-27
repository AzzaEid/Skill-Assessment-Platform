using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Application.Services;
using System.Security.Claims;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CreationAssignmentsController : ControllerBase
    {
        private readonly CreationAssignmentService _assignmentService;
        private readonly IResponseHandler _responseHandler;

        public CreationAssignmentsController(
            CreationAssignmentService assignmentService,
            IResponseHandler responseHandler)
        {
            _assignmentService = assignmentService;
            _responseHandler = responseHandler;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assignmentService.GetAll();
            return _responseHandler.Success(result);
        }
        [Authorize(Roles = "SeniorExaminer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDTO dto)
        {
            var result = await _assignmentService.CreateAssignmentAsync(dto);
            return _responseHandler.Created(result, "Assignment created successfully");
        }
        [Authorize(Roles = "SeniorExaminer")]
        [HttpGet("senior")]
        public async Task<IActionResult> GetBySenior()
        {
            var seniorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (seniorId == null)
                return _responseHandler.Unauthorized();
            var result = await _assignmentService.GetBySeniorId(seniorId);
            return _responseHandler.Success(result, "Assignment created successfully");
        }

        [HttpGet("examiner/{examinerId}")]
        public async Task<IActionResult> GetByExaminer(string examinerId)
        {
            var result = await _assignmentService.GetExaminerAssignmentsAsync(examinerId);
            return _responseHandler.Success(result);
        }
        [HttpGet("senior/{seniorId}/overdue")]
        public async Task<IActionResult> GetOverDueBySeniior(string seniorId)
        {
            var result = await _assignmentService.GetOverdueBySenior(seniorId);
            return _responseHandler.Success(result);
        }


        [Authorize(Roles = "SeniorExaminer")]
        [HttpPut("exam-completed/{assignmentId}")]
        public async Task<IActionResult> UpdateExamStatus(int assignmentId)
        {
            await _assignmentService.UpdateExamCreationAssignmentStatusAsync(assignmentId);
            return _responseHandler.Success(null, "Exam assignment status updated successfully");
        }

        [Authorize(Roles = "SeniorExaminer")]
        [HttpPut("cancel/{assignmentId}")]
        public async Task<IActionResult> CancelAssignment(int assignmentId)
        {
            await _assignmentService.CancelCreationAssignmentStatusAsync(assignmentId);
            return _responseHandler.Success(null, "Assignment cancelled successfully");
        }
    }

}
