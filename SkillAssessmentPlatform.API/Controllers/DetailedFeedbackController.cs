using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{
    using SkillAssessmentPlatform.API.Common;
    using SkillAssessmentPlatform.Application.DTOs;
    [ApiController]
    [Route("api/[controller]")]
    public class DetailedFeedbackController : ControllerBase
    {
        private readonly DetailedFeedbackService _detailedFeedbackService;
        private readonly IResponseHandler _responseHandler;

        public DetailedFeedbackController(DetailedFeedbackService detailedFeedbackService, IResponseHandler responseHandler)
        {
            _detailedFeedbackService = detailedFeedbackService;
            _responseHandler = responseHandler;
        }

        [HttpGet("by-feedback/{feedbackId}")]
        public async Task<IActionResult> GetByFeedbackId(int feedbackId)
        {
            var result = await _detailedFeedbackService.GetByFeedbackIdAsync(feedbackId);
            return _responseHandler.Success(result);
        }
    }

}
