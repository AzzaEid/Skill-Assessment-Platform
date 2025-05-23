using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.API.Common;
namespace SkillAssessmentPlatform.API.Controllers
{
    using SkillAssessmentPlatform.API.Common;

    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;
        private readonly IResponseHandler _responseHandler;

        public FeedbackController(FeedbackService feedbackService, IResponseHandler responseHandler)
        {
            _feedbackService = feedbackService;
            _responseHandler = responseHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateFeedbackDTO dto)
        {
            var result = await _feedbackService.CreateAsync(dto);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _feedbackService.GetByIdAsync(id);
            if (result == null)
                return _responseHandler.NotFound("Feedback not found");
            return _responseHandler.Success(result);
        }

        [HttpGet("by-examiner/{examinerId}")]
        public async Task<IActionResult> GetByExaminerId(string examinerId)
        {
            var result = await _feedbackService.GetByExaminerIdAsync(examinerId);
            return _responseHandler.Success(result);
        }
    }


}
