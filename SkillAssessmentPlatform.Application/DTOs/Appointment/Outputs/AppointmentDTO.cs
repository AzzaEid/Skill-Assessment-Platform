namespace SkillAssessmentPlatform.Application.DTOs.Appointment.Outputs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBooked { get; set; }
        public string ExaminerName { get; set; }
    }
}
