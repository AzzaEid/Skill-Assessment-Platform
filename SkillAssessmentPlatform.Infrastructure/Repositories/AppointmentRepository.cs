using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ILogger<AppointmentRepository> _logger;

        public AppointmentRepository(
            AppDbContext context,
            ILogger<AppointmentRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Appointment>> GetAvailableAppointmentsByExaminerIdAsync(string examinerId)
        {

            return await _context.Appointments
                .Where(a => a.ExaminerId == examinerId && !a.IsBooked)
                .Where(a => a.StartTime > DateTime.Now) // Only future appointments
                .Include(a => a.Examiner)
                .OrderBy(a => a.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetAvailableAppointmentsForDateRangeAsync(string examinerId, DateTime startDate, DateTime endDate)
        {

            return await _context.Appointments
                .Where(a => a.ExaminerId == examinerId && !a.IsBooked)
                .Where(a => a.StartTime >= startDate && a.StartTime <= endDate)
                .OrderBy(a => a.StartTime)
                .Include(a => a.Examiner)
                .ToListAsync();

        }

        public async Task<bool> IsAppointmentAvailableAsync(int appointmentId)
        {

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            return !appointment.IsBooked && appointment.StartTime > DateTime.Now;

        }

        public async Task<bool> MarkAppointmentAsBookedAsync(int appointmentId)
        {

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            if (appointment.IsBooked)
                throw new BadRequestException("This appointment is already booked");

            appointment.IsBooked = true;
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> MarkAppointmentAsAvailableAsync(int appointmentId)
        {

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            if (!appointment.IsBooked)
                throw new BadRequestException("This appointment is already available");

            appointment.IsBooked = false;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
