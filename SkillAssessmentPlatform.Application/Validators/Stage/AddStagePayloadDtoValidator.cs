using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class AddStagePayloadDtoValidator : AbstractValidator<AddStagePayloadDto>
    {
        public AddStagePayloadDtoValidator()
        {
            RuleFor(x => x.MainStage)
                .NotNull().WithMessage("Main stage is required")
                .SetValidator(new CreateStageWithDetailsDTOValidator());

            RuleForEach(x => x.AdditionalStages)
                .SetValidator(new CreateStageWithDetailsDTOValidator())
                .When(x => x.AdditionalStages != null);
        }
    }
}
