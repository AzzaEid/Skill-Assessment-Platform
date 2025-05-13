namespace SkillAssessmentPlatform.Core.Results
{
    public class DaySchedule
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
