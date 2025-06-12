using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Auth
{
    public class UserRegisterDTOValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterDTOValidator()
        {
            RuleFor(x => x.Email)
                .IsValidEmail();

            RuleFor(x => x.Password)
                .IsValidPassword();

            RuleFor(x => x.FullName)
                .IsValidFullName();
        }
    }
}
