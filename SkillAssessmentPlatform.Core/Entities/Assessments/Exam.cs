using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class Exam
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public int DurationMinutes { get; set; }
        public string Difficulty { get; set; }
        public string QuestionsType { get; set; }
        public bool IsActive { get; set; } = true;


        // Navigation properties
        public Stage Stage { get; set; }
        public ICollection<ExamRequest> ExamRequests { get; set; } = new HashSet<ExamRequest>();
    }
}
