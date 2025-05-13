using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs.Appointment;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;
using SkillAssessmentPlatform.Core.Results;

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

        public async Task<IEnumerable<AppointmentDTO>> GetAvailableAppointmentsAsync(string examinerId, DateTime startDate, DateTime endDate)
        {
            try
            {

                var appointments = await _unitOfWork.AppointmentRepository.GetAvailableAppointmentsAsync(examinerId, startDate, endDate);
                if (appointments == null)
                {
                    throw new KeyNotFoundException($"There's no appointments for examiner {examinerId}");
                }
                return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available appointments");
                throw;
            }
        }
        public async Task<IEnumerable<DateSlotDTO>> GetAvailableSlotsAsync(string examinerId, DateTime startDate, DateTime endDate)
        {

            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(examinerId);
            if (examiner == null)
                throw new KeyNotFoundException($"There's no appointments for examiner {examinerId}");

            return await _unitOfWork.AppointmentRepository.GetAvailableSlotsAsync(examinerId, startDate, endDate);

        }
        public async Task<IEnumerable<AppointmentDTO>> CreateBulkAppointmentsAsync(AppointmentCreateDTO bulkDTO)
        {
            try
            {
                var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(bulkDTO.ExaminerId);
                if (examiner == null)
                    throw new KeyNotFoundException($"Appointment with id {bulkDTO.ExaminerId} not found");
                // Validations
                if (bulkDTO.EndDate < bulkDTO.StartDate)
                    throw new BadRequestException("End date must be after start date");

                if (bulkDTO.StartDate < DateTime.Now.Date)
                    throw new BadRequestException("Start date cannot be in the past");

                if (bulkDTO.SlotDurationMinutes <= 0)
                    throw new BadRequestException("Slot duration must be greater than 0");

                var appointments = await _unitOfWork.AppointmentRepository.CreateBulkAppointmentsAsync(bulkDTO);
                return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bulk appointments");
                throw;
            }
        }
        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int id)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {id} not found");

            var appointmentDTO = _mapper.Map<AppointmentDTO>(appointment);
            return appointmentDTO;
        }



        public async Task<AppointmentDTO>? CreateAppointmentAsync(AppointmentSingleCreateDTO appointmentDTO)
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


        public async Task<IEnumerable<DateSlotDTO>> GetApplicantAvailableSlotsAsync(string applicantId, int stageId)
        {
            try
            {
                // الحصول على Stage Progress للمتقدم
                var stageProgress = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(applicantId, stageId);

                if (stageProgress == null)
                    throw new KeyNotFoundException($"No stage progress found for applicant {applicantId} and stage {stageId}");

                // الحصول على معلومات المقابلة المرتبطة بالمرحلة
                var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);

                if (stage == null || stage.Type != StageType.Interview)
                    throw new BadRequestException("Stage is not an interview type");

                var interview = await _unitOfWork.InterviewRepository.GetByStageIdAsync(stageId);

                if (interview == null)
                    throw new KeyNotFoundException($"No interview configuration found for stage {stageId}");

                // حساب تاريخ انتهاء الفترة المسموح بها للحجز
                var startDate = stageProgress.StartDate;
                var endDate = startDate.AddDays(interview.MaxDaysToBook);

                // الحصول على المختبر المسؤول عن هذه المرحلة
                var examiner = stageProgress.ExaminerId;

                if (string.IsNullOrEmpty(examiner))
                    throw new BadRequestException("No examiner assigned for this stage");

                // الحصول على المواعيد المتاحة
                return await _unitOfWork.AppointmentRepository.GetAvailableSlotsAsync(examiner, startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting available slots for applicant {applicantId} and stage {stageId}");
                throw;
            }
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
