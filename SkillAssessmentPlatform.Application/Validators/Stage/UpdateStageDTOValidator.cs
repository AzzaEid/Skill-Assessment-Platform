using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class UpdateStageDTOValidator : AbstractValidator<UpdateStageDTO>
    {
        public UpdateStageDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(1000);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid stage type");

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0");

            RuleFor(x => x.PassingScore)
                .InclusiveBetween(0, 100)
                .WithMessage("Passing score must be between 0 and 100");

            RuleFor(x => x.NoOfattempts)
                .InclusiveBetween(1, 10)
                .WithMessage("Number of attempts must be between 1 and 10");
        }
    }
}
