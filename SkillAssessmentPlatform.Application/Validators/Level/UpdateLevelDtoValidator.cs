using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Level
{
    public class UpdateLevelDtoValidator : AbstractValidator<UpdateLevelDto>
    {
        public UpdateLevelDtoValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(1000);

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0");
        }
    }
}
