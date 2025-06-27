using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Level.Input;
using SkillAssessmentPlatform.Application.Validators.Base;
using SkillAssessmentPlatform.Application.Validators.EvaluationCriteria;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class StageCreateDTOValidator : AbstractValidator<StageCreateDTO>
    {
        public StageCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(1000);

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Stage type is required")
                .Must(type => new[] { "Exam", "Interview", "Task" }.Contains(type))
                .WithMessage("Stage type must be Exam, Interview, or Task");

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0");

            RuleFor(x => x.PassingScore)
                .InclusiveBetween(0, 100)
                .WithMessage("Passing score must be between 0 and 100");

            RuleFor(x => x.EvaluationCriteria)
                .NotEmpty().WithMessage("At least one evaluation criteria must be provided");

            RuleForEach(x => x.EvaluationCriteria)
                .SetValidator(new CriteriaCreateDTOValidator());
        }
    }

}
