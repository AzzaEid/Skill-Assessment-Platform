using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.StageProgress.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.StageProgress
{
    public class UpdateStageStatusDTOValidator : AbstractValidator<UpdateStageStatusDTO>
    {
        public UpdateStageStatusDTOValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid applicant result status");

            RuleFor(x => x.Score)
                .IsValidScore();
        }
    }
}
