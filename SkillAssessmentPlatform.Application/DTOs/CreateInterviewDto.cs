namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateInterviewDto
    {

        public int StageId { get; set; }
        public int MaxDaysToBook { get; set; }
        public int DurationMinutes { get; set; }
        public string Instructions { get; set; }


    }
}
