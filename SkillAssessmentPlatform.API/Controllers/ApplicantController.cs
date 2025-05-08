using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantsController : ControllerBase
    {
        private readonly ApplicantService _applicantService;
        private readonly IResponseHandler _responseHandler;

        public ApplicantsController(
            ApplicantService applicantService,
            IResponseHandler responseHandler)
        {
            _applicantService = applicantService;
            _responseHandler = responseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _applicantService.GetAllApplicantsAsync(page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var applicant = await _applicantService.GetApplicantByIdAsync(id);
            return _responseHandler.Success(applicant);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            string id,
            [FromBody] UpdateStatusDTO updateStatusDto)
        {
            var updatedApplicant = await _applicantService.UpdateApplicantStatusAsync(id, updateStatusDto);
            return _responseHandler.Success(updatedApplicant, "Applicant status updated");
        }
        /*
        [HttpGet("{applicantId}/enrollments")]
        public async AppTask<IActionResult> GetEnrollments(
            string applicantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _applicantService.GetApplicantEnrollmentsAsync(applicantId, page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpGet("{applicantId}/certificates")]
        public async AppTask<IActionResult> GetCertificates(
            string applicantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _applicantService.GetApplicantCertificatesAsync(applicantId, page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpPost("{applicantId}/enrollments")]
        public async AppTask<IActionResult> EnrollInTrack(
            string applicantId,
            [FromBody] EnrollmentCreateDTO enrollmentDto)
        {
            var enrollment = await _applicantService.EnrollApplicantInTrackAsync(applicantId, enrollmentDto);
            return _responseHandler.Created(enrollment, "Enrollment created successfully");
        }

        [HttpGet("{applicantId}/progress")]
        public async AppTask<IActionResult> GetProgress(
            string applicantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _applicantService.GetApplicantProgressAsync(applicantId, page, pageSize);
            return _responseHandler.Success(result);
        }
        */
        
    }

}