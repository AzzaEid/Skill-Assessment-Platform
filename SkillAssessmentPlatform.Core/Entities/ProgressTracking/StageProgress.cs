using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;

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

        // ✅ FIX: Add ApplicantId
        public string ApplicantId { get; set; }

        // Navigation properties
        public Examiner Examiner { get; set; }
        public Stage Stage { get; set; }
        public LevelProgress LevelProgress { get; set; }
        public Applicant Applicant { get; set; } // Optional nav property (useful if needed later)
    }
}
