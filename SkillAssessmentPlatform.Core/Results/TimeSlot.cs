namespace SkillAssessmentPlatform.Core.Results
{
    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AppointmentId { get; set; }
        public bool IsBooked { get; set; }
    }
}
