namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class Interview
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public int MaxDaysToBook { get; set; }
        public int DurationMinutes { get; set; }
        public string Instructions { get; set; }
        public bool IsActive { get; set; } = true;


        // Navigation properties
        public Stage Stage { get; set; }
        public ICollection<InterviewBook> InterviewBooks { get; set; } = new List<InterviewBook>();
    }
}
