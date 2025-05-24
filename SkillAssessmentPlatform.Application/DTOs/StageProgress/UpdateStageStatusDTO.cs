using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.StageProgress
{
    public class UpdateStageStatusDTO
    {
        public ProgressStatus Status { get; set; }
        public int Score { get; set; }
    }
}
