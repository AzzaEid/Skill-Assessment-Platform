using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Track
{

    public class CreateAssociatedSkillDTOValidator : AbstractValidator<CreateAssociatedSkillDTO>
    {
        public CreateAssociatedSkillDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required")
                .MaximumLength(100).WithMessage("Skill name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .IsValidDescription(500);
        }
    }
}
