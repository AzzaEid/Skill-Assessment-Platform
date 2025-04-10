using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class Stage
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }  // "Exam", "Task", "Interview"
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public int PassingScore { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Level Level { get; set; }
        public ICollection<EvaluationCriteria> EvaluationCriteria { get; set; }
        public ICollection<StageProgress> StageProgresses { get; set; }
        public Interview Interview { get; set; }
        public Exam Exam { get; set; }
        public ICollection<TasksPool> TasksPools { get; set; }
    }
}