namespace SkillAssessmentPlatform.Core.Results
{
    public class AppointmentCreateDTO
    {
        // معلومات إنشاء مواعيد متكررة
        public string ExaminerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DaySchedule> WeeklySchedule { get; set; } = new List<DaySchedule>();
        public int SlotDurationMinutes { get; set; } = 30;
    }
}
