using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class LevelProgress
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int LevelId { get; set; }
        public ProgressStatus Status { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime CompletionDate { get; set; }

        // Navigation properties
        public Level Level { get; set; }
        public Enrollment Enrollment { get; set; }
        public ICollection<StageProgress> StageProgresses { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
    }
}
