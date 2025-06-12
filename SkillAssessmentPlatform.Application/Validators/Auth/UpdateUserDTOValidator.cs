using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Auth
{
    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            RuleFor(x => x.FullName)
                .IsValidFullName();

            RuleFor(x => x.DateOfBirth)
                .Must(date => !date.HasValue || date.Value < DateTime.Now)
                .WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender value");
        }
    }
}
