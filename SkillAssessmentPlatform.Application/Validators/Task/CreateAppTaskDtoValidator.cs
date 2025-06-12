using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Task
{
    public class CreateAppTaskDtoValidator : AbstractValidator<CreateAppTaskDto>
    {
        public CreateAppTaskDtoValidator()
        {
            RuleFor(x => x.TaskPoolId)
                .IsValidId();

            RuleFor(x => x.Title)
                .IsValidTitle();

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .IsValidDescription(2000);

            RuleFor(x => x.Requirements)
                .NotEmpty().WithMessage("Requirements are required")
                .IsValidDescription(1000);

            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("Difficulty level is required")
                .Must(d => new[] { "Easy", "Medium", "Hard" }.Contains(d))
                .WithMessage("Difficulty must be Easy, Medium, or Hard");
        }
    }
}
