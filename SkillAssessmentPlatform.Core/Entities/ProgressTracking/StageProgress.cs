using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class StageProgress
    {
        public int Id { get; set; }
        public int LevelProgressId { get; set; }
        public int StageId { get; set; }
        public ProgressStatus Status { get; set; }  
        public int Score { get; set; }
        public string ExaminerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public int Attempts { get; set; }


        // Navigation properties
        // add by the relation bett stageProg && EXaminer && Stage
        public Examiner Examiner { get; set; }
        public Stage Stage { get; set; }
        public LevelProgress LevelProgress { get; set; }
        public ICollection<TaskApplicant> TaskApplicants { get; set; }
    }
}
