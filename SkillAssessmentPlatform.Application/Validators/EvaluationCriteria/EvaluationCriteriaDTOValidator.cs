using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Output;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.EvaluationCriteria
{
    public class EvaluationCriteriaDTOValidator : AbstractValidator<EvaluationCriteriaDTO>
    {
        public EvaluationCriteriaDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(500);

            RuleFor(x => x.Weight)
                .IsValidWeight();
        }
    }
}
