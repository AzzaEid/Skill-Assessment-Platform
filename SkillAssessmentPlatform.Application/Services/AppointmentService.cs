using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs.Appointment;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class AppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AppointmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResponse<AppointmentDTO>> GetAllAppointmentsAsync(int page = 1, int pageSize = 10)
        {
            var appointmentsQuery = _unitOfWork.AppointmentRepository
                .GetPagedQueryable(page, pageSize)
                .Include(a => a.Examiner);
            var totalCount = await _unitOfWork.AppointmentRepository.GetTotalCountAsync();
            var appointments = await appointmentsQuery.ToListAsync();

            var appointmentDTOs = _mapper.Map<List<AppointmentDTO>>(appointments);

            return new PagedResponse<AppointmentDTO>(
                appointmentDTOs,
                page,
                pageSize,
                totalCount
                );

        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {id} not found");

            var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            return appointmentDTO;
        }


        public async Task<IEnumerable<AppointmentDTO>> GetAvailableAppointmentsByExaminerAsync(string examinerId)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Appointment with id {examinerId} not found");

            var appointments = await _unitOfWork.AppointmentRepository.GetAvailableAppointmentsByExaminerIdAsync(examinerId);
            var appointmentDTOs = _mapper.Map<List<AppointmentDTO>>(appointments);

            return appointmentDTOs;

        }

        public async Task<IEnumerable<AppointmentDTO>> GetAvailableAppointmentsForDateRangeAsync(
            string examinerId, DateTime startDate, DateTime endDate)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Appointment with id {examinerId} not found");

            var appointments = await _unitOfWork.AppointmentRepository.GetAvailableAppointmentsForDateRangeAsync(
                examinerId, startDate, endDate);

            var appointmentDTOs = _mapper.Map<List<AppointmentDTO>>(appointments);

            return appointmentDTOs;
        }


        public async Task<AppointmentDTO>? CreateAppointmentAsync(AppointmentCreateDTO appointmentDTO)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(appointmentDTO.ExaminerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentDTO.ExaminerId} not found");

            // Validate appointment
            if (appointmentDTO.EndTime <= appointmentDTO.StartTime || appointmentDTO.StartTime < DateTime.Now)
                return null; // Validate End time and start time


            var appointment = _mapper.Map<Appointment>(appointmentDTO);
            appointment.IsBooked = false;

            await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDTO>(appointment);

        }

        public async Task<IEnumerable<AppointmentDTO>> CreateBulkAppointmentsAsync(AppointmentBulkCreateDTO bulkDTO)
        {
            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(bulkDTO.ExaminerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Appointment with id {bulkDTO.ExaminerId} not found");
            // Validations
            if (bulkDTO.EndDate < bulkDTO.StartDate)
                throw new BadRequestException("End date must be after start date");

            if (bulkDTO.StartDate < DateTime.Now.Date)
                throw new BadRequestException("Start date cannot be in the past");

            if (bulkDTO.EndHour <= bulkDTO.StartHour)
                throw new BadRequestException("End hour must be after start hour");

            if (bulkDTO.SlotDurationMinutes <= 0)
                throw new BadRequestException("Slot duration must be greater than 0");

            var appointments = new List<Appointment>();

            for (DateTime date = bulkDTO.StartDate; date <= bulkDTO.EndDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Friday)
                    continue;

                for (int hour = bulkDTO.StartHour; hour < bulkDTO.EndHour; hour++)
                {
                    for (int minute = 0; minute < 60; minute += bulkDTO.SlotDurationMinutes)
                    {
                        var startTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
                        var endTime = startTime.AddMinutes(bulkDTO.SlotDurationMinutes);

                        if (endTime.Hour >= bulkDTO.EndHour)
                            break;

                        appointments.Add(new Appointment
                        {
                            ExaminerId = bulkDTO.ExaminerId,
                            StartTime = startTime,
                            EndTime = endTime,
                            IsBooked = false
                        });
                    }
                }
            }

            await _unitOfWork.AppointmentRepository.AddRangeAsync(appointments);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id)
                              ?? throw new KeyNotFoundException($"Appointment with id {id} not found");

            if (appointment.IsBooked)
                throw new BadRequestException("Cannot delete a booked appointment");

            if (appointment.StartTime <= DateTime.Now)
                throw new BadRequestException("Cannot delete a past appointment");

            return await _unitOfWork.AppointmentRepository.DeleteAsync(id);
        }
    }
}
