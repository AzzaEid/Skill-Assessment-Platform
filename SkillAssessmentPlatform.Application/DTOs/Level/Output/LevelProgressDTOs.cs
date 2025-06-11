using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class LevelProgressDTO
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public ProgressStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        // public LevelDetailDto Level { get; set; }
        public string Description { get; set; }
        public int StagesCount { get; set; } = 0;
        public int StagesProgressesCount { get; set; } = 0;
    }

    public class UpdateLevelStatusDTO
    {
        public ProgressStatus Status { get; set; }
    }
}
