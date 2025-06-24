using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Exceptions;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Core.Results;
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

        public async Task<IEnumerable<Appointment>> GetAvailableAppointmentsAsync(string examinerId, DateTime startDate, DateTime endDate)
        {

            return await _context.Appointments
               .Where(a => a.ExaminerId == examinerId &&
                      a.StartTime >= startDate &&
                      a.EndTime <= endDate &&
                      !a.IsBooked)
               .Include(a => a.Examiner)
               .OrderBy(a => a.StartTime)
               .ToListAsync();

        }
        public async Task<IEnumerable<DateSlotDTO>> GetAvailableSlotsAsync(string examinerId, DateTime startDate, DateTime endDate)
        {
            var appointments = await _context.Appointments
                .Where(a => a.ExaminerId == examinerId &&
                       a.StartTime >= startDate &&
                       a.EndTime <= endDate)
                .OrderBy(a => a.StartTime)
                .ToListAsync();

            // تجميع المواعيد حسب التاريخ
            var groupedByDate = appointments
                .GroupBy(a => a.StartTime.Date)
                .OrderBy(g => g.Key);

            var result = new List<DateSlotDTO>();

            foreach (var group in groupedByDate)
            {
                var dateSlot = new DateSlotDTO
                {
                    Date = group.Key,
                    Slots = group.Select(a => new TimeSlot
                    {
                        StartTime = a.StartTime,
                        EndTime = a.EndTime,
                        AppointmentId = a.Id,
                        IsBooked = a.IsBooked
                    }).ToList()
                };
                result.Add(dateSlot);
            }

            return result;
        }
        public async Task<IEnumerable<Appointment>> CreateBulkAppointmentsAsync(AppointmentCreateDTO createDto)
        {
            if (createDto.SlotDurationMinutes <= 0)
                throw new ArgumentException("Slot duration must be greater than 0.");

            if (createDto.WeeklySchedule == null || !createDto.WeeklySchedule.Any())
                return Enumerable.Empty<Appointment>();

            var appointments = new List<Appointment>();
            var currentDate = createDto.StartDate.Date;
            var endDate = createDto.EndDate.Date;

            while (currentDate <= endDate)
            {
                var daySchedule = createDto.WeeklySchedule
                    .FirstOrDefault(d => d.DayOfWeek == currentDate.DayOfWeek);

                if (daySchedule != null)
                {
                    var startTimeOfDay = new DateTime(
                        currentDate.Year, currentDate.Month, currentDate.Day,
                        daySchedule.StartTime.Hours, daySchedule.StartTime.Minutes, 0,
                        DateTimeKind.Local).ToUniversalTime();

                    var endTimeOfDay = new DateTime(
                        currentDate.Year, currentDate.Month, currentDate.Day,
                        daySchedule.EndTime.Hours, daySchedule.EndTime.Minutes, 0,
                        DateTimeKind.Local).ToUniversalTime();

                    var currentSlotStart = startTimeOfDay;

                    while (currentSlotStart.AddMinutes(createDto.SlotDurationMinutes) <= endTimeOfDay)
                    {
                        var appointment = new Appointment
                        {
                            ExaminerId = createDto.ExaminerId,
                            StartTime = currentSlotStart,
                            EndTime = currentSlotStart.AddMinutes(createDto.SlotDurationMinutes),
                            IsBooked = false
                        };

                        appointments.Add(appointment);

                        currentSlotStart = currentSlotStart
                            .AddMinutes(createDto.SlotDurationMinutes + 10);
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            await _context.Appointments.AddRangeAsync(appointments);
            await _context.SaveChangesAsync();

            return appointments;
        }

        public async Task<bool> IsAppointmentAvailableAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            return appointment != null && !appointment.IsBooked;
        }

        public async Task<Appointment> MarkAppointmentAsBookedAsync(int appointmentId)
        {

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            if (appointment.IsBooked)
                throw new BadRequestException("This appointment is already booked");

            appointment.IsBooked = true;
            await _context.SaveChangesAsync();
            return appointment;

        }

        public async Task<Appointment> MarkAppointmentAsAvailableAsync(int appointmentId)
        {

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {appointmentId} not found");

            appointment.IsBooked = false;
            await _context.SaveChangesAsync();
            return appointment;

        }
    }
}
