using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook;
using SkillAssessmentPlatform.Core.Common;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class InterviewBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<InterviewBookService> _logger;
        private readonly NotificationService _notificationService;
        private readonly EmailService _emailService;
        private readonly IMeetingService _meetingService;
        private readonly StageProgressService _stageProgressService;


        public InterviewBookService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<InterviewBookService> logger,
            NotificationService notificationService,
            EmailService emailService,
            IMeetingService meetingService,
            StageProgressService stageProgressService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _notificationService = notificationService;
            _emailService = emailService;
            _meetingService = meetingService;
            _stageProgressService = stageProgressService;
        }

        public async Task<PagedResponse<InterviewBookDTO>> GetAllInterviewBooksAsync(int page = 1, int pageSize = 10)
        {

            var interviewBooks = _unitOfWork.InterviewBookRepository.GetPagedQueryable(page, pageSize)
                .Include(b => b.Applicant).Include(b => b.Appointment);
            var totalCount = await _unitOfWork.InterviewBookRepository.GetTotalCountAsync();

            var interviewBookDTOs = _mapper.Map<List<InterviewBookDTO>>(interviewBooks);

            return new PagedResponse<InterviewBookDTO>(
                interviewBookDTOs,
                page,
                pageSize,
                totalCount
            );

        }

        public async Task<InterviewBookDTO> GetInterviewBookByIdAsync(int id)
        {

            var interviewBook = await _unitOfWork.InterviewBookRepository.GetByIdAsync(id);
            if (interviewBook == null)
                throw new KeyNotFoundException($"Interview booking with id {id} not found");

            var interviewBookDTO = _mapper.Map<InterviewBookDTO>(interviewBook);

            return interviewBookDTO;

        }

        public async Task<IEnumerable<InterviewBookDTO>> GetInterviewBooksByApplicantIdAsync(string applicantId)
        {

            var interviewBooks = await _unitOfWork.InterviewBookRepository.GetByApplicantIdAsync(applicantId);
            var interviewBookDTOs = _mapper.Map<List<InterviewBookDTO>>(interviewBooks);

            return interviewBookDTOs;

        }

        public async Task<IEnumerable<InterviewBookDTO>> GetInterviewBooksByExaminerIdAsync(string examinerId)
        {

            var interviewBooks = await _unitOfWork.InterviewBookRepository.GetByExaminerIdAsync(examinerId);
            var interviewBookDTOs = _mapper.Map<List<InterviewBookDTO>>(interviewBooks);

            return interviewBookDTOs;
        }

        public async Task<IEnumerable<InterviewBookDTO>> GetInterviewBooksByStageIdAsync(int stageId)
        {
            try
            {
                var interviewBooks = await _unitOfWork.InterviewBookRepository.GetByStageIdAsync(stageId);
                var interviewBookDTOs = _mapper.Map<List<InterviewBookDTO>>(interviewBooks);

                return interviewBookDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting interview bookings for stage {stageId}");
                throw;
            }
        }
        //==>>>
        public async Task<InterviewBookDTO> BookInterviewAsync(string applicantId, InterviewBookCreateDTO bookingDto)
        {
            try
            {
                var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(applicantId);
                if (applicant == null)
                    throw new KeyNotFoundException($"Applicant with id {applicantId} not found");
                // check appointment
                var isAvailable = await _unitOfWork.AppointmentRepository.IsAppointmentAvailableAsync(bookingDto.AppointmentId);
                if (!isAvailable)
                    throw new BadRequestException("This appointment is no longer available");

                // interview details
                var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(bookingDto.InterviewId);
                if (interview == null)
                    throw new KeyNotFoundException($"Interview with id {bookingDto.InterviewId} not found");

                // Create InterviewBook
                var interviewBook = new InterviewBook
                {
                    InterviewId = bookingDto.InterviewId,
                    AppointmentId = bookingDto.AppointmentId,
                    ApplicantId = applicantId,
                    Status = InterviewStatus.Pending
                };
                var createdBooking = await _unitOfWork.InterviewBookRepository.CreateInterviewBookAsync(interviewBook);

                // Notify 
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(bookingDto.AppointmentId);
                await NotifyExaminerAboutBookingAsync(appointment.ExaminerId, createdBooking.Id);

                return _mapper.Map<InterviewBookDTO>(createdBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking interview");
                throw;
            }
        }
        public async Task<InterviewBookDTO> ApproveBookingAsync(int bookingId)
        {
            try
            {

                var booking = await _unitOfWork.InterviewBookRepository.GetByIdAsync(bookingId);
                if (booking == null)
                    throw new KeyNotFoundException($"Booking with id {bookingId} not found");


                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(booking.AppointmentId);


                var interview = await _unitOfWork.InterviewRepository.GetByIdAsync(booking.InterviewId);



                var meetingTopic = $"Interview for Stage {interview.StageId}";
                var zoomMeeting = await _meetingService.CreateMeetingAsync(appointment.StartTime, appointment.EndTime, meetingTopic);


                booking.Status = InterviewStatus.Scheduled;
                booking.MeetingLink = zoomMeeting.StartUrl;

                await _unitOfWork.InterviewBookRepository.UpdateAsync(booking);

                await SendInterviewScheduledNotificationsAsync(booking, appointment, interview);

                return _mapper.Map<InterviewBookDTO>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving booking {bookingId}");
                throw;
            }
        }
        private async Task NotifyExaminerAboutBookingAsync(string examinerId, int bookingId)
        {
            // Notify examiner
            await _notificationService.SendNotificationAsync(
                examinerId,
                NotificationType.NewInterviewBooking,
                $"You have a new interview booking request. Please review and approve/reject it."
            );

            // Email examiner about bokking
            var examiner = await _unitOfWork.UserRepository.GetByIdAsync(examinerId);
            await _emailService.SendEmailAsync(
                examiner.Email,
                "New Interview Booking Request",
                $"Dear {examiner.FullName},<br><br>You have a new interview booking request. Please review and approve/reject it through the system.<br><br>Best regards,<br>The Team"
            );
        }
        public async Task<InterviewBookDTO> RejectBookingAsync(int bookingId)
        {
            try
            {
                var booking = await _unitOfWork.InterviewBookRepository.GetByIdAsync(bookingId);
                if (booking == null)
                    throw new KeyNotFoundException($"Booking with id {bookingId} not found");

                booking.Status = InterviewStatus.Canceled;
                await _unitOfWork.InterviewBookRepository.UpdateAsync(booking);

                // change appointment status
                var appointment = await _unitOfWork.AppointmentRepository.MarkAppointmentAsAvailableAsync(booking.AppointmentId);
                /*
                var stage = await _unitOfWork.StageRepository.GetByInterviewId(booking.InterviewId);
                var sp = await _unitOfWork.StageProgressRepository.GetByApplicantAndStageAsync(booking.ApplicantId, stage.Id);
                if (sp == null)
                {
                    throw new Exception("error in update applicant progress");
                }

                // تحديث الستيج بروغريس
                await _stageProgressService.UpdateStatusAsync(sp.Id,
                    new UpdateStageStatusDTO { Score = 0, Status = ApplicantResultStatus.Failed });
                */
                // Notify examiner
                await _notificationService.SendNotificationAsync(
                    booking.ApplicantId,
                    NotificationType.InterviewBookingRejected,
                    $"Your interview booking for {appointment.StartTime:dd/MM/yyyy HH:mm} has been rejected. Please book another appointment."
                );

                // send email for applicant
                var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(booking.ApplicantId);
                await _emailService.SendEmailAsync(
                    applicant.Email,
                    "Interview Booking Rejected",
                    $"Dear {applicant.FullName},<br><br>Your interview booking for {appointment.StartTime:dd/MM/yyyy HH:mm} has been rejected. Please book another appointment.<br><br>Best regards,<br>The Team"
                );

                return _mapper.Map<InterviewBookDTO>(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting booking {bookingId}");
                throw;
            }
        }
        private async Task SendInterviewScheduledNotificationsAsync(InterviewBook booking, Appointment appointment, Interview interview)
        {
            var applicant = await _unitOfWork.ApplicantRepository.GetByIdAsync(booking.ApplicantId);
            var examiner = await _unitOfWork.UserRepository.GetByIdAsync(appointment.ExaminerId);

            // notificaton
            await _notificationService.SendNotificationAsync(
                booking.ApplicantId,
               NotificationType.InterviewScheduled,
                $"Your interview has been scheduled for {appointment.StartTime:dd/MM/yyyy HH:mm}. A meeting link has been sent to your email."
            );

            await _notificationService.SendNotificationAsync(
                appointment.ExaminerId,
                NotificationType.InterviewConfirmed,
                $"An interview with {applicant.FullName} has been confirmed for {appointment.StartTime:dd/MM/yyyy HH:mm}."
            );
            //emal
            await _emailService.SendEmailAsync(
                applicant.Email,
                "Interview Scheduled",
                $"Dear {applicant.FullName},<br><br>Your interview has been scheduled for {appointment.StartTime:dd/MM/yyyy HH:mm}.<br><br>You can join the meeting using this link: <a href='{booking.MeetingLink}'>{booking.MeetingLink}</a><br><br>Best regards,<br>The Team"
            );

            await _emailService.SendEmailAsync(
                examiner.Email,
                "Interview Confirmed",
                $"Dear {examiner.FullName},<br><br>An interview with {applicant.FullName} has been confirmed for {appointment.StartTime:dd/MM/yyyy HH:mm}.<br><br>You can join the meeting using this link: <a href='{booking.MeetingLink}'>{booking.MeetingLink}</a><br><br>Best regards,<br>The Team"
            );
        }
    }
}