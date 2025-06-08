using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly AppCertificateService _certificateService;
        private readonly IResponseHandler _responseHandler;
        private readonly PdfGeneratorService _pdfService;
        private readonly IUnitOfWork _unitOfWork;

        public CertificatesController(
            AppCertificateService certificateService,
            IResponseHandler responseHandler,
            PdfGeneratorService pdfService,
            IUnitOfWork unitOfWork)
        {
            _certificateService = certificateService;
            _responseHandler = responseHandler;
            _pdfService = pdfService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("verify/{code}")]
        public async Task<IActionResult> Verify(string code)
        {
            var result = await _certificateService.VerifyByCodeAsync(code);
            return result == null
                ? _responseHandler.NotFound("Invalid or expired verification code")
                : _responseHandler.Success(result);
        }

        //  جميع الشهادات التي حصل عليها المتقدم
        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetByApplicantId(string applicantId)
        {
            var result = await _certificateService.GetByApplicantIdAsync(applicantId);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cert = await _certificateService.GetByIdAsync(id);
            return cert == null
                ? _responseHandler.NotFound("Certificate not found")
                : _responseHandler.Success(cert);
        }

        //  عرض الشهادة كرابط HTML مباشر
        [HttpGet("html/{code}")]
        public async Task<IActionResult> GetCertificateByCode(string code)
        {
            var certificate = await _unitOfWork.AppCertificateRepository
                .GetByVerificationCodeAsync(code);

            if (certificate == null)
                return NotFound("Invalid verification code");

            var applicant = await _unitOfWork.ApplicantRepository
                .GetByIdAsync(certificate.ApplicantId);

            var levelProgress = await _unitOfWork.LevelProgressRepository
                .GetByIdWithLevelAndTrack(certificate.LeveProgressId);

            var html = CertificateHtmlBuilder.Build(
                applicant.FullName,
                levelProgress.Level.Track.Name,
                levelProgress.Level.Name,
                certificate.IssueDate,
                certificate.VerificationCode
            );

            return Content(html, "text/html");
        }

        // تحميل الشهادة كملف PDF
        [HttpGet("{code}/pdf")]
        public async Task<IActionResult> GetCertificatePdf(string code)
        {
            var certificate = await _unitOfWork.AppCertificateRepository
                .GetByVerificationCodeAsync(code);

            if (certificate == null)
                return NotFound("Invalid verification code");

            var applicant = await _unitOfWork.ApplicantRepository
                .GetByIdAsync(certificate.ApplicantId);

            var levelProgress = await _unitOfWork.LevelProgressRepository
                .GetByIdWithLevelAndTrack(certificate.LeveProgressId);

            var html = CertificateHtmlBuilder.Build(
                applicant.FullName,
                levelProgress.Level.Track.Name,
                levelProgress.Level.Name,
                certificate.IssueDate,
                certificate.VerificationCode
            );

            var pdfBytes = _pdfService.GeneratePdfFromHtml(html);

            return File(pdfBytes, "application/pdf", "certificate.pdf");
        }
    }
}
