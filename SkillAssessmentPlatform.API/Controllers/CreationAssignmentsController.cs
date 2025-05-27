using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Enums;

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
        [Authorize(Roles = "SeniorExaminer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDTO dto)
        {
            var result = await _assignmentService.CreateAssignmentAsync(dto);
            return _responseHandler.Created(result, "Assignment created successfully");
        }

        [HttpGet("examiner/{examinerId}")]
        public async Task<IActionResult> GetByExaminer(string examinerId)
        {
            var result = await _assignmentService.GetExaminerAssignmentsAsync(examinerId);
            return _responseHandler.Success(result);
        }
        [Authorize(Roles = "Examiner")]
        [HttpPut("task-progress/{taskId}")]
        public async Task<IActionResult> UpdateTaskCreationProgress(int taskId)
        {
            await _assignmentService.UpdateTaskCreationProgressAsync(taskId);
            return _responseHandler.Success(null, "Task creation progress updated successfully");
        }
        [Authorize(Roles = "SeniorExaminer")]
        [HttpPut("exam-status/{assignmentId}")]
        public async Task<IActionResult> UpdateExamStatus(int assignmentId, [FromQuery] AssignmentStatus newStatus)
        {
            await _assignmentService.UpdateExamCreationAssignmentStatusAsync(assignmentId, newStatus);
            return _responseHandler.Success(null, "Exam assignment status updated successfully");
        }

        [Authorize(Roles = "SeniorExaminer")]
        [HttpPut("cancel/{assignmentId}")]
        public async Task<IActionResult> CancelAssignment(int assignmentId, [FromQuery] AssignmentStatus newStatus)
        {
            await _assignmentService.CancelCreationAssignmentStatusAsync(assignmentId, newStatus);
            return _responseHandler.Success(null, "Assignment cancelled successfully");
        }
    }

}
