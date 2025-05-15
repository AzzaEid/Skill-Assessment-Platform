namespace SkillAssessmentPlatform.Core.Results
{
    public class DateSlotDTO
    {
        public DateTime Date { get; set; }
        public List<TimeSlot> Slots { get; set; } = new List<TimeSlot>();
    }
}
