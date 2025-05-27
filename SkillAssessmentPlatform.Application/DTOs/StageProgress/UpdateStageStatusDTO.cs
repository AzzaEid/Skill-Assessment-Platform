using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.StageProgress
{
    public class UpdateStageStatusDTO
    {
        public ApplicantResultStatus Status { get; set; }
        public decimal Score { get; set; }
    }
}
