using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Applicant.Input;

namespace SkillAssessmentPlatform.Application.Validators.Applicant
{
    public class UpdateStatusDTOValidator : AbstractValidator<UpdateStatusDTO>
    {
        public UpdateStatusDTOValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid applicant status");
        }
    }
}
