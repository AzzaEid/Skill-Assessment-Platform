using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class CreateStageListDTOValidator : AbstractValidator<CreateStageListDTO>
    {
        public CreateStageListDTOValidator()
        {
            RuleFor(x => x.Stages)
                .NotEmpty().WithMessage("At least one stage must be provided");

            RuleForEach(x => x.Stages)
                .SetValidator(new CreateStageWithDetailsDTOValidator());
        }
    }
}
