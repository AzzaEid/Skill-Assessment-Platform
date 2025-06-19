using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Applicant.Input;
using SkillAssessmentPlatform.Application.Services;

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


    }

}