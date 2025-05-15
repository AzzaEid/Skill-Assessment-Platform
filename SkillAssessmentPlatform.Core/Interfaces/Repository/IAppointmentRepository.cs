using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Results;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAvailableAppointmentsAsync(string examinerId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<DateSlotDTO>> GetAvailableSlotsAsync(string examinerId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<Appointment>> CreateBulkAppointmentsAsync(AppointmentCreateDTO createDto);
        Task<bool> IsAppointmentAvailableAsync(int appointmentId);
        Task<Appointment> MarkAppointmentAsBookedAsync(int appointmentId);
        Task<Appointment> MarkAppointmentAsAvailableAsync(int appointmentId);
    }
}
