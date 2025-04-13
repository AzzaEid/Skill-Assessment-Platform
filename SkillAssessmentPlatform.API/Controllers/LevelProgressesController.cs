using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    
    [ApiController]
    [Route("api/level-progresses")]
    public class LevelProgressesController : ControllerBase
    {
        private readonly LevelProgressService _levelProgressService;
        private readonly IResponseHandler _responseHandler;

        public LevelProgressesController(
            LevelProgressService levelProgressService,
            IResponseHandler responseHandler)
        {
            _levelProgressService = levelProgressService;
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var levelProgress = await _levelProgressService.GetByIdAsync(id);
            return _responseHandler.Success(levelProgress);
        }

        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<IActionResult> GetByEnrollmentId(int enrollmentId)
        {
            var levelProgresses = await _levelProgressService.GetByEnrollmentIdAsync(enrollmentId);
            return _responseHandler.Success(levelProgresses);
        }

        [HttpGet("enrollment/{enrollmentId}/current")]
        public async Task<IActionResult> GetCurrentLevelProgress(int enrollmentId)
        {
            var currentLevelProgress = await _levelProgressService.GetCurrentLevelProgressAsync(enrollmentId);
            return _responseHandler.Success(currentLevelProgress);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateLevelStatusDTO updateDto)
        {
            var updatedLevelProgress = await _levelProgressService.UpdateStatusAsync(id, updateDto);
            return _responseHandler.Success(updatedLevelProgress, "Level progress status updated successfully");
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicantId(string applicantId)
        {
            var levelProgresses = await _levelProgressService.GetByApplicantIdAsync(applicantId);
            return _responseHandler.Success(levelProgresses);
        }
    }
}
