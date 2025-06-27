using SkillAssessmentPlatform.Application.DTOs.Exam.Input;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.Structure
{
    public class StageStructureDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public StageType Type { get; set; }
        public int? Order { get; set; }
        public int? PassingScore { get; set; }
        public int? NoOfAttempts { get; set; }
        public CreateInterviewDetailsDto? Interview { get; set; }
        public CreateExamDetailsDto? Exam { get; set; }
        public CreateTasksPoolDetailsDto? TasksPool { get; set; }
        public List<EvaluationStructureCriteriaDTO?> EvaluationCriteria { get; set; }
    }

}
