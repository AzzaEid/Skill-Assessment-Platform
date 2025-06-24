using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.CachedServices;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Outputs;
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
        private readonly ICacheService _cacheService;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICacheService cacheService,
            ILogger<AppointmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAvailableAppointmentsAsync(string examinerId, DateTime startDate, DateTime endDate)
        {


            var appointments = await _unitOfWork.AppointmentRepository.GetAvailableAppointmentsAsync(examinerId, startDate, endDate);
            if (appointments == null)
            {
                throw new KeyNotFoundException($"There's no appointments for examiner {examinerId}");
            }
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);

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

            var examiner = await _unitOfWork.ExaminerRepository.GetByIdAsync(bulkDTO.ExaminerId);
            if (examiner == null)
                throw new KeyNotFoundException($"Appointment with id {bulkDTO.ExaminerId} not found");
            // Validations
            if (bulkDTO.EndDate < bulkDTO.StartDate)
                throw new BadRequestException("End date must be after start date");

            if (bulkDTO.StartDate < DateTime.UtcNow.Date)
                throw new BadRequestException("Start date cannot be in the past");

            if (bulkDTO.SlotDurationMinutes <= 0)
                throw new BadRequestException("Slot duration must be greater than 0");

            var appointments = await _unitOfWork.AppointmentRepository.CreateBulkAppointmentsAsync(bulkDTO);
            return _mapper.Map<IEnumerable<AppointmentDTO>>(appointments);

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

            if (appointmentDTO.EndTime <= appointmentDTO.StartTime)
                throw new BadRequestException("End time must be after start time");

            if (appointmentDTO.StartTime <= DateTime.UtcNow)
                throw new BadRequestException("Start time must be in the future");


            var appointment = _mapper.Map<Appointment>(appointmentDTO);
            appointment.IsBooked = false;
            appointment.EndTime = appointmentDTO.EndTime.ToUniversalTime();
            appointment.StartTime = appointmentDTO.StartTime.ToUniversalTime();


            await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AppointmentDTO>(appointment);

        }


        public async Task<IEnumerable<DateSlotDTO>> GetApplicantAvailableSlotsAsync(string applicantId, int stageId)
        {
            try
            {
                var stageProgress = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(applicantId, stageId);

                if (stageProgress == null)
                    throw new KeyNotFoundException($"No stage progress found for applicant {applicantId} and stage {stageId}");

                var stage = await _unitOfWork.StageRepository.GetByIdAsync(stageId);

                if (stage == null || stage.Type != StageType.Interview)
                    throw new BadRequestException("Stage is not an interview type");

                var interview = await _unitOfWork.InterviewRepository.GetByStageIdAsync(stageId);

                if (interview == null)
                    throw new KeyNotFoundException($"No interview configuration found for stage {stageId}");

                var startDate = stageProgress.StartDate;
                var endDate = startDate.AddDays(interview.MaxDaysToBook);

                var examiner = stageProgress.ExaminerId;

                if (string.IsNullOrEmpty(examiner))
                    throw new BadRequestException("No examiner assigned for this stage");

                var cacheKey = string.Format(CacheKeys.AVAILABLE_SLOTS, examiner, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                return await _cacheService.GetOrCreateAsync(
                    cacheKey,
                    () => _unitOfWork.AppointmentRepository.GetAvailableSlotsAsync(examiner, startDate, endDate),
                    CacheKeys.SLOTS_CACHE_DURATION
                );
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

            if (appointment.StartTime <= DateTime.UtcNow)
                throw new BadRequestException("Cannot delete a past appointment");

            return await _unitOfWork.AppointmentRepository.DeleteAsync(id);
        }
    }
}
