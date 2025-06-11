namespace SkillAssessmentPlatform.Application.DTOs.Exam.Output
{
    public class ExamDto
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        public bool IsActive { get; set; }


        public List<string> QuestionsType { get; set; }
    }
}
