using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class InterviewBookRepository : GenericRepository<InterviewBook>, IInterviewBookRepository
    {
        private readonly ILogger<InterviewBookRepository> _logger;
        private readonly IAppointmentRepository _appointmentRepository;

        public InterviewBookRepository(
            AppDbContext context,
            ILogger<InterviewBookRepository> logger,
            IAppointmentRepository appointmentRepository) : base(context)
        {
            _logger = logger;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IEnumerable<InterviewBook>> GetByApplicantIdAsync(string applicantId)
        {

            return await _context.InterviewBooks
                .Where(ib => ib.ApplicantId == applicantId)
                .Include(ib => ib.Applicant)
                .Include(ib => ib.Interview)
                .Include(ib => ib.Appointment)
                .OrderByDescending(ib => ib.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InterviewBook>> GetByExaminerIdAsync(string examinerId)
        {
            return await _context.InterviewBooks
                .Include(ib => ib.Appointment)
                .Where(ib => ib.Appointment.ExaminerId == examinerId)
                .Include(ib => ib.Applicant)
                .Include(ib => ib.Interview)
                .OrderByDescending(ib => ib.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InterviewBook>> GetByStageIdAsync(int stageId)
        {
            return await _context.InterviewBooks
                .Include(ib => ib.Interview)
                .Where(ib => ib.Interview.StageId == stageId)
                .Include(ib => ib.Applicant)
                .Include(ib => ib.Appointment)
                .OrderByDescending(ib => ib.ScheduledDate)
                .ToListAsync();
        }
        /*
        public async Task<InterviewBook> CreateInterviewBookAsync(string applicantId, int interviewId, int appointmentId)
        {

            // Check if appointment is available نقل على السيرفس
            bool isAvailable = await _appointmentRepository.IsAppointmentAvailableAsync(appointmentId);
            if (!isAvailable)
                throw new BadRequestException("The selected appointment is not available");

            // Get appointment to set scheduled date
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            // Check if the interview exists
            var interview = await _context.Interviews.FindAsync(interviewId);
            if (interview == null)
                throw new KeyNotFoundException($"Interview with id {interviewId} not found");

            // Create the interview booking
            var interviewBook = new InterviewBook
            {
                InterviewId = interviewId,
                AppointmentId = appointmentId,
                ApplicantId = applicantId,
                ScheduledDate = appointment.StartTime,
                Status = InterviewStatus.Pending
            };

            // Mark the appointment as booked
            await _appointmentRepository.MarkAppointmentAsBookedAsync(appointmentId);

            // Save the booking
            await _context.InterviewBooks.AddAsync(interviewBook);
            await _context.SaveChangesAsync();

            // Generate meeting link
            return await GenerateMeetingLinkAsync(interviewBook.Id);

        }
        */
        public async Task<InterviewBook> UpdateInterviewStatusAsync(int interviewBookId, InterviewStatus status)
        {

            var interviewBook = await _context.InterviewBooks.FindAsync(interviewBookId);
            if (interviewBook == null)
                throw new KeyNotFoundException($"Interview booking with id {interviewBookId} not found");


            // If status is changing to Canceled, make the appointment available again
            if (status == InterviewStatus.Canceled)
            {
                await _appointmentRepository.MarkAppointmentAsAvailableAsync(interviewBook.AppointmentId);
            }

            interviewBook.Status = status;
            _context.InterviewBooks.Update(interviewBook);
            await _context.SaveChangesAsync();

            return interviewBook;
        }

        public async Task<InterviewBook> CreateInterviewBookAsync(InterviewBook interviewBook)
        {
            // التأكد من أن الموعد متاح
            var appointment = await _context.Appointments.FindAsync(interviewBook.AppointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {interviewBook.AppointmentId} not found");

            if (appointment.IsBooked)
                throw new BadRequestException("This appointment is already booked");

            // تحديث حالة الموعد
            appointment.IsBooked = true;
            _context.Appointments.Update(appointment);

            // إضافة حجز المقابلة
            await _context.InterviewBooks.AddAsync(interviewBook);
            await _context.SaveChangesAsync();

            return interviewBook;
        }
        public async Task<InterviewBook> GenerateMeetingLinkAsync(int interviewBookId)
        {
            try
            {
                var interviewBook = await _context.InterviewBooks
                    .Include(ib => ib.Interview)
                    .Include(ib => ib.Appointment)
                    .FirstOrDefaultAsync(ib => ib.Id == interviewBookId);

                if (interviewBook == null)
                    throw new KeyNotFoundException($"Interview booking with id {interviewBookId} not found");

                // Generate a Google Meet link
                // In a real application this would integrate with Google Calendar API
                // For now, create a placeholder link
                string meetingId = Guid.NewGuid().ToString("N").Substring(0, 12);
                interviewBook.MeetingLink = $"https://meet.google.com/{meetingId}";

                await _context.SaveChangesAsync();
                return interviewBook;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating meeting link for booking {interviewBookId}");
                throw;
            }
        }

        public async Task<InterviewBook> GetByStageProgressIdAsync(int stageProgressId)
        {
            return await _context.InterviewBooks
                .Where(ib => ib.Interview.Stage.StageProgresses
                    .Any(sp => sp.Id == stageProgressId && sp.LevelProgress.Enrollment.ApplicantId == ib.ApplicantId))
                .Include(ib => ib.Appointment)
                .OrderByDescending(ib => ib.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<InterviewBook> GetPendingByApplicantIdAsync(string applicantId)
        {
            return await _context.InterviewBooks
                .Where(ib => ib.ApplicantId == applicantId && ib.Status == InterviewStatus.Pending)
                .Include(ib => ib.Appointment)
                .FirstOrDefaultAsync();
        }
        public async Task<InterviewBook> GetScheduledByApplicantIdAsync(string applicantId)
        {
            return await _context.InterviewBooks
                .Where(ib => ib.ApplicantId == applicantId && ib.Status == InterviewStatus.Scheduled)
                .Include(ib => ib.Appointment)
                .FirstOrDefaultAsync();
        }

    }
}
