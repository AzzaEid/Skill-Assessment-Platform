using FluentValidation;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.LevelStage
{
    public class CreateLevelDTOValidator : AbstractValidator<CreateLevelDTO>
    {
        public CreateLevelDTOValidator()
        {
            When(x => !string.IsNullOrEmpty(x.Name), () =>
            {
                RuleFor(x => x.Name)
                    .IsValidTitle(100);
            });

            When(x => !string.IsNullOrEmpty(x.Description), () =>
            {
                RuleFor(x => x.Description)
                    .IsValidDescription(1000);
            });

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0")
                .When(x => x.Order.HasValue);

        }
    }

}
