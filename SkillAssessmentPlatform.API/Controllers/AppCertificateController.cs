using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly AppCertificateService _certificateService;
        private readonly IResponseHandler _responseHandler;

        public CertificatesController(AppCertificateService certificateService, IResponseHandler responseHandler)
        {
            _certificateService = certificateService;
            _responseHandler = responseHandler;
        }

        [HttpGet("verify/{code}")]
        public async Task<IActionResult> Verify(string code)
        {
            var result = await _certificateService.VerifyByCodeAsync(code);
            return result == null
                ? _responseHandler.NotFound("Invalid or expired verification code")
                : _responseHandler.Success(result);
        }
        // all cer. which the app. got
        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicantId(string applicantId)
        {
            var result = await _certificateService.GetByApplicantIdAsync(applicantId);
            return _responseHandler.Success(result);
        }
        // for one cer. by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cert = await _certificateService.GetByIdAsync(id);
            return cert == null
                ? _responseHandler.NotFound("Certificate not found")
                : _responseHandler.Success(cert);
        }


    }

}
