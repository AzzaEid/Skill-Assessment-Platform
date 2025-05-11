using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<IEnumerable<Appointment>> GetAvailableAppointmentsByExaminerIdAsync(string examinerId);
        Task<IEnumerable<Appointment>> GetAvailableAppointmentsForDateRangeAsync(string examinerId, DateTime startDate, DateTime endDate);
        Task<bool> IsAppointmentAvailableAsync(int appointmentId);
        Task<bool> MarkAppointmentAsBookedAsync(int appointmentId);
        Task<bool> MarkAppointmentAsAvailableAsync(int appointmentId);
    }
}
