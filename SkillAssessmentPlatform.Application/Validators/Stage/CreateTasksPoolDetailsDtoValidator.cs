using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class CreateTasksPoolDetailsDtoValidator : AbstractValidator<CreateTasksPoolDetailsDto>
    {
        public CreateTasksPoolDetailsDtoValidator()
        {
            RuleFor(x => x.DaysToSubmit)
                .InclusiveBetween(1, 30)
                .WithMessage("Days to submit must be between 1 and 30");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .IsValidDescription(1000);

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Requirements are required")
                .IsValidDescription(500);
        }
    }
}
