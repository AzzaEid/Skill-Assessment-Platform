using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Auth
{
    public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordDTOValidator()
        {
            RuleFor(x => x.Email)
                .IsValidEmail();

            RuleFor(x => x.Password)
                .IsValidPassword();

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Reset token is required");
        }
    }

}
