using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input;
using SkillAssessmentPlatform.Application.Services;

namespace SkillAssessmentPlatform.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class InterviewBookingsController : ControllerBase
    {
        private readonly InterviewBookService _interviewBookService;
        private readonly IResponseHandler _responseHandler;

        public InterviewBookingsController(
            InterviewBookService interviewBookService,
            IResponseHandler responseHandler)
        {
            _interviewBookService = interviewBookService;
            _responseHandler = responseHandler;
        }

        [HttpPost("{applicantId}")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> BookInterview(string applicantId, [FromBody] InterviewBookCreateDTO bookingDto)
        {
            var booking = await _interviewBookService.BookInterviewAsync(applicantId, bookingDto);
            return _responseHandler.Created(booking, "Interview booking created successfully");
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Examiner,SeniorExaminer")]
        public async Task<IActionResult> ApproveBooking(int id)
        {
            var booking = await _interviewBookService.ApproveBookingAsync(id);
            return _responseHandler.Success(booking, "Interview booking approved successfully");
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Examiner,SeniorExaminer")]
        public async Task<IActionResult> RejectBooking(int id)
        {
            var booking = await _interviewBookService.RejectBookingAsync(id);
            return _responseHandler.Success(booking, "Interview booking rejected successfully");
        }

        [HttpGet("applicant/{applicantId}")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> GetApplicantBookings(string applicantId)
        {
            var bookings = await _interviewBookService.GetInterviewBooksByApplicantIdAsync(applicantId);
            return _responseHandler.Success(bookings);
        }

        [HttpGet("examiner/{examinerId}")]
        [Authorize(Roles = "Examiner,SeniorExaminer")]
        public async Task<IActionResult> GetExaminerBookings(string examinerId)
        {
            var bookings = await _interviewBookService.GetInterviewBooksByExaminerIdAsync(examinerId);
            return _responseHandler.Success(bookings);
        }
        [HttpGet("stage/{stageId}")]
        [Authorize(Roles = "Examiner,SeniorExaminer")]
        public async Task<IActionResult> GetExaminerBookings(int stageId)
        {
            var bookings = await _interviewBookService.GetInterviewBooksByStageIdAsync(stageId);
            return _responseHandler.Success(bookings);
        }
    }
}
