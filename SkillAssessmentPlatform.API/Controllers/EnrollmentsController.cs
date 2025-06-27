using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Enrollment;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly IResponseHandler _responseHandler;

        public EnrollmentsController(
            EnrollmentService enrollmentService,
            IResponseHandler responseHandler)
        {
            _enrollmentService = enrollmentService;
            _responseHandler = responseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _enrollmentService.GetAllEnrollmentsAsync(page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(id);
            return _responseHandler.Success(enrollment);
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicantId(
            string applicantId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var enrollments = await _enrollmentService.GetByApplicantIdAsync(applicantId, page, pageSize);
            return _responseHandler.Success(enrollments);
        }

        [HttpPost("applicant/{applicantId}")]
        public async Task<IActionResult> EnrollApplicant(
            string applicantId,
            [FromBody] EnrollmentCreateDTO enrollmentDto)
        {
            var enrollment = await _enrollmentService.EnrollApplicantInTrackAsync(applicantId, enrollmentDto);
            return _responseHandler.Created(enrollment, "Enrollment created successfully");
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateEnrollmentStatusDTO updateDto)
        {
            var updatedEnrollment = await _enrollmentService.UpdateEnrollmentStatusAsync(id, updateDto);
            return _responseHandler.Success(updatedEnrollment, "Enrollment status updated successfully");
        }
    }
}
