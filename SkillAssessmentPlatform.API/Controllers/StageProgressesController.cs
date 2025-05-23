using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Exceptions;
using System.Security.Claims;

namespace SkillAssessmentPlatform.API.Controllers
{

    [ApiController]
    [Route("api/stage-progresses")]
    [Produces("application/json")]
    public class StageProgressesController : ControllerBase
    {
        private readonly StageProgressService _stageProgressService;
        private readonly IResponseHandler _responseHandler;

        public StageProgressesController(
            StageProgressService stageProgressService,
            IResponseHandler responseHandler)
        {
            _stageProgressService = stageProgressService;
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stageProgress = await _stageProgressService.GetByIdAsync(id);
            return _responseHandler.Success(stageProgress);
        }

        [HttpGet("level-progress/{levelprogressId}")]
        public async Task<IActionResult> GetBylevelprogressIdd(int levelprogressId)
        {
            var stageProgresses = await _stageProgressService.GetByLevelProgressIdAsync(levelprogressId);
            return _responseHandler.Success(stageProgresses);
        }
        [HttpGet("level-progress/{levelprogressId}/current")]
        public async Task<IActionResult> GetCurrentStageProgress(int levelprogressId)
        {
            var currentStageProgress = await _stageProgressService.GetCurrentStageProgressAsync(levelprogressId);
            return _responseHandler.Success(currentStageProgress);
        }

        [HttpGet("enrollment/{enrollmentId}/current")]
        public async Task<IActionResult> GetCurrentStageProgressbyEnrollment(int enrollmentId)
        {
            var currentStageProgress = await _stageProgressService.GetByCurrEnrollmentIdAsync(enrollmentId);
            return _responseHandler.Success(currentStageProgress);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateStageStatusDTO updateDto)
        {
            var updatedStageProgress = await _stageProgressService.UpdateStatusAsync(id, updateDto);
            return _responseHandler.Success(updatedStageProgress, "Stage progress status updated successfully");
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicantId(string applicantId)
        {
            var stageProgresses = await _stageProgressService.GetByApplicantIdAsync(applicantId);
            return _responseHandler.Success(stageProgresses);
        }

        [HttpGet("applicant/{applicantId}/current")]
        public async Task<IActionResult> GetCurrentStageForApplicant(string applicantId)
        {
            var currentStage = await _stageProgressService.GetCurrentStageForApplicantAsync(applicantId);
            return _responseHandler.Success(currentStage);
        }

        [HttpGet("applicant/{applicantId}/completed")]
        public async Task<IActionResult> GetCompletedStages(string applicantId)
        {
            var completedStages = await _stageProgressService.GetCompletedStagesAsync(applicantId);
            return _responseHandler.Success(completedStages);
        }

        [HttpGet("applicant/{applicantId}/failed")]
        public async Task<IActionResult> GetFailedStages(string applicantId)
        {
            var failedStages = await _stageProgressService.GetFailedStagesAsync(applicantId);
            return _responseHandler.Success(failedStages);
        }

        [HttpPost("enrollment/{enrollmentId}/stage/{stageId}/attempt")]
        public async Task<IActionResult> CreateNewAttempt(int enrollmentId, int stageId)
        {
            var newAttempt = await _stageProgressService.CreateNewAttemptAsync(enrollmentId, stageId);
            return _responseHandler.Created(newAttempt, "New attempt created successfully");
        }

        [HttpGet("stage/{stageId}/attempts")]
        public async Task<IActionResult> GetAttemptCount(int stageId)
        {
            var attemptCount = await _stageProgressService.GetAttemptCountAsync(stageId);
            return _responseHandler.Success(new { attemptCount });
        }
        /*
        [HttpPost("enrollment/{enrollmentId}/stage/{currentStageId}/next")]
        public async AppTask<IActionResult> CreateNextStageProgress(int enrollmentId, int currentStageId)
        {
            var nextStageProgress = await _stageProgressService.CreateNextStageProgressAsync(enrollmentId, currentStageId);
            return _responseHandler.Created(nextStageProgress, "Next stage progress created successfully");
        }
        /*/
        [HttpPut("{id}/examiner")]
        public async Task<IActionResult> AssignExaminer(
            int id,
            [FromBody] AssignExaminerDTO assignDto)
        {
            var updatedStageProgress = await _stageProgressService.AssignExaminerAsync(id, assignDto);
            return _responseHandler.Success(updatedStageProgress, "Examiner assigned successfully");
        }
    }
}