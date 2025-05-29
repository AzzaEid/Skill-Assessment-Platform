using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class StageDetailDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public StageType Type { get; set; } // or use Enum
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public double PassingScore { get; set; }
        public int NoOfattempts { get; set; }
        public List<EvaluationCriteriaDTO> EvaluationCriteria { get; set; }
        public ExamDto Exam { get; set; }
        public InterviewDto Interview { get; set; }
        public TasksPoolDto TasksPool { get; set; }

    }

}