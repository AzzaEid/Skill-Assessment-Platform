using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.EvaluationCriteria
{
    public class CreateEvaluationCriteriaDtoValidator : AbstractValidator<CreateEvaluationCriteriaDto>
    {
        public CreateEvaluationCriteriaDtoValidator()
        {
            RuleFor(x => x.StageId)
                .IsValidId();

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Criteria name is required")
                .MaximumLength(100).WithMessage("Criteria name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .IsValidDescription(500);

            RuleFor(x => x.Weight)
                .IsValidWeight();
        }
    }
}
