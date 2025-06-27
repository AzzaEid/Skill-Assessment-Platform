namespace SkillAssessmentPlatform.Application.DTOs.Exam.Input
{
    public class UpdateExamDto
    {

        public int Id { get; set; }
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        public List<string> QuestionsType { get; set; }

    }
}
