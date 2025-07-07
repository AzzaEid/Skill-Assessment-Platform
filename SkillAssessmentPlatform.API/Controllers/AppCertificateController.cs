using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.API.Helpers;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    // [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : Controller
    {
        private readonly AppCertificateService _certificateService;
        private readonly IResponseHandler _responseHandler;
        private readonly IPdfGeneratorService _pdfService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ViewRender _viewRender;

        public CertificatesController(
            AppCertificateService certificateService,
            IResponseHandler responseHandler,
            IPdfGeneratorService pdfService,
            IServiceProvider serviceProvider,
            ViewRender viewRender)
        {
            _certificateService = certificateService;
            _responseHandler = responseHandler;
            _pdfService = pdfService;
            _serviceProvider = serviceProvider;
            _viewRender = viewRender;
        }
        /*
        [HttpGet("verify/{code}")]
        public async Task<IActionResult> Verify(string code)
        {
            var result = await _certificateService.VerifyByCodeAsync(code);
            return result == null
                ? _responseHandler.NotFound("Invalid or expired verification code")
                : _responseHandler.Success(result);
        }/*/
        [HttpGet("{code}")]
        public async Task<IActionResult> VerifyCertificate(string code)
        {
            var certificate = await _certificateService.VerifyByCodeAsync(code);
            if (certificate == null)
                return NotFound("Invalid or expired verification code");
            return View("Display", certificate);
        }
        // عرض الشهادة كصفحة HTML
        [HttpGet("view")]
        public async Task<IActionResult> ViewCertificate([FromQuery] string applicantId, [FromQuery] int levelId)
        {
            try
            {
                var code = await _certificateService.GetByLevelIdAndApplicantId(levelId, applicantId);
                if (code == null)
                    return NotFound("Invalid or expired verification code");

                return RedirectToAction(nameof(VerifyCertificate), new { code = code });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        /*
            //  عرض الشهادة كرابط HTML مباشر
            [HttpGet("html/{code}")]
            public async Task<IActionResult> GetCertificateByCode(string code)
            {
                var certificate = await _certificateService.VerifyByCodeAsync(code);

                if (certificate == null)
                    return NotFound("Invalid verification code");

                var html = CertificateHtmlBuilder.Build(
                    certificate.ApplicantName,
                    certificate.TrackName,
                    certificate.LevelName,
                    certificate.IssueDate,
                    certificate.VerificationCode
                );

                return Content(html, "text/html");
            }
        /*/
        // تحميل الشهادة كملف PDF
        [HttpGet("{code}/pdf")]
        public async Task<IActionResult> DownloadCertificatePdf(string code)
        {
            try
            {
                var certificate = await _certificateService.VerifyByCodeAsync(code);
                if (certificate == null)
                    return NotFound("Invalid or expired verification code");

                // توليد HTML من الـ View
                var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);
                var htmlContent = await _viewRender.RenderViewToStringAsync(actionContext, "Display", certificate);

                // تحويل HTML إلى PDF
                var pdfBytes = _pdfService.GeneratePdfFromHtml(htmlContent);

                var fileName = $"Certificate_{certificate.ApplicantName}_{certificate.VerificationCode}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating PDF: {ex.Message}");
            }
        }

    }
}

