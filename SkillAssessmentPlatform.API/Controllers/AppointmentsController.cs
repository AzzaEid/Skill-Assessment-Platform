using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.DTOs.Appointment;
using SkillAssessmentPlatform.Application.Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _appointmentService.GetAllAppointmentsAsync(page, pageSize);
            return _responseHandler.Success(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            return _responseHandler.Success(appointment);
        }

        [HttpGet("examiner/{examinerId}/available")]
        public async Task<IActionResult> GetAvailableByExaminer(string examinerId)
        {
            var appointments = await _appointmentService.GetAvailableAppointmentsByExaminerAsync(examinerId);
            return _responseHandler.Success(appointments);
        }

        [HttpGet("examiner/{examinerId}/timerange")]
        public async Task<IActionResult> GetAvailableByDateRange(
            string examinerId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var appointments = await _appointmentService.GetAvailableAppointmentsForDateRangeAsync(
                examinerId, startDate, endDate);
            return _responseHandler.Success(appointments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SeniorExaminer")]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDTO appointmentDto)
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
        public async Task<IActionResult> CreateBulk([FromBody] AppointmentBulkCreateDTO bulkDto)
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