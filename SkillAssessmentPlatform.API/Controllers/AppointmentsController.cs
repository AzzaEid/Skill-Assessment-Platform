using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Appointment;
using SkillAssessmentPlatform.Application.Services;
using SkillAssessmentPlatform.Core.Results;

namespace SkillAssessmentPlatform.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly IResponseHandler _responseHandler;

        public AppointmentsController(
            AppointmentService appointmentService,
            IResponseHandler responseHandler)
        {
            _appointmentService = appointmentService;
            _responseHandler = responseHandler;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return _responseHandler.Success(appointment);
        }

        /*  [HttpGet("examiner/{examinerId}/available")]
          public async Task<IActionResult> GetAvailableByExaminer(string examinerId)
          {
              var appointments = await _appointmentService.GetAvailableAppointmentsByExaminerAsync(examinerId);
              return _responseHandler.Success(appointments);
          }
        */
        [HttpGet("slots/applicant/{applicantId}/stage/{stageId}")]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> GetApplicantAvailableSlots(string applicantId, int stageId)
        {
            var slots = await _appointmentService.GetApplicantAvailableSlotsAsync(applicantId, stageId);
            return _responseHandler.Success(slots);
        }
        [HttpGet("slots/examiner/{examinerId}")]
        public async Task<IActionResult> GetExaminerAvailableSlots(
        string examinerId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(examinerId, startDate, endDate);
            return _responseHandler.Success(slots);
        }

        [HttpGet("examiner/{examinerId}/timerange")]
        public async Task<IActionResult> GetAvailableByDateRange(
            string examinerId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var appointments = await _appointmentService.GetAvailableAppointmentsAsync(
                examinerId, startDate, endDate);
            return _responseHandler.Success(appointments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> Create([FromBody] AppointmentSingleCreateDTO appointmentDto)
        {
            var appointment = await _appointmentService.CreateAppointmentAsync(appointmentDto);
            if (appointment == null)
            {
                return _responseHandler.BadRequest("Validate End time and start time");
            }
            return _responseHandler.Created(appointment, "Appointment created successfully");
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> CreateBulk([FromBody] AppointmentCreateDTO bulkDto)
        {
            var appointments = await _appointmentService.CreateBulkAppointmentsAsync(bulkDto);
            return _responseHandler.Created(appointments, "Appointments created successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);
            return _responseHandler.Success(null, "Appointment deleted successfully");
        }
    }
}