namespace SkillAssessmentPlatform.Application.DTOs.Exam.Input
{
    public class CreateExamDetailsDto
    {
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        //public QuestionType QuestionsType { get; set; }
        public List<string> QuestionsType { get; set; }

    }
}
