using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Level.Input;
using SkillAssessmentPlatform.Application.Validators.Base;
using SkillAssessmentPlatform.Application.Validators.Stage;

namespace SkillAssessmentPlatform.Application.Validators.Level
{
    public class LevelCreateDTOValidator : AbstractValidator<LevelCreateDTO>
    {
        public LevelCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(1000);

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0");

            RuleFor(x => x.Stages)
                .NotEmpty().WithMessage("At least one stage must be provided");

            RuleForEach(x => x.Stages)
                .SetValidator(new StageCreateDTOValidator());
        }
    }
}
