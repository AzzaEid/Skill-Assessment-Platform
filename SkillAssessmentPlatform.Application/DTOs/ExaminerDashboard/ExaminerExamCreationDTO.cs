using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerExamCreationDTO
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public string Difficulty { get; set; }
        public QuestionType RequiredQuestionTypes { get; set; }
        public DateTime AssignedDate { get; set; }
        public int DaysRemaining { get; set; }
    }
}
