using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class Stage
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StageType Type { get; set; }  // "Exam", "Task", "Interview"
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
        public int PassingScore { get; set; }
        public int NoOfattempts { get; set; } = 3;

        // Navigation properties
        public Level Level { get; set; }
        public ICollection<EvaluationCriteria> EvaluationCriteria { get; set; }
        public ICollection<StageProgress> StageProgresses { get; set; }
        public Interview Interview { get; set; }
        public Exam Exam { get; set; }
        public ICollection<TasksPool> TasksPools { get; set; }
    }
}