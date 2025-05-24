using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Services;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateFeedbackDTO dto)
        {
            var result = await _feedbackService.UpdateAsync(id, dto);
            if (result == null)
                return _responseHandler.NotFound("Feedback not found");
            return _responseHandler.Success(result, "Feedback updated");
        }
        /*
                [HttpGet("progress/{id}")]
                public async Task<IActionResult> GetByProgressId(int id)
                {
                    var result = await _feedbackService.GetByProgressIdAsync(id);
                    if (result == null)
                        return _responseHandler.NotFound("No feedback for this progress ID");
                    return _responseHandler.Success(result);
                }
        */
        [HttpDelete("{feedbackId}")]
        public async Task<IActionResult> Delete(int feedbackId)
        {
            var success = await _feedbackService.DeleteAsync(feedbackId);
            return success
                ? _responseHandler.Success("Feedback deleted successfully", null)
                : _responseHandler.NotFound("Feedback not found");
        }






    }


}