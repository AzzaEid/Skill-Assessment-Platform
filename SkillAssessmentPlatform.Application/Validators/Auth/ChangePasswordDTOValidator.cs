using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Auth
{
    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(x => x.Email)
                .IsValidEmail();

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .IsValidPassword();

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.OldPassword)
                .WithMessage("New password must be different from current password");
        }
    }
}
