using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Feedback
{
    public class CreateDetailedFeedbackDTOValidator : AbstractValidator<CreateDetailedFeedbackDTO>
    {
        public CreateDetailedFeedbackDTOValidator()
        {
            RuleFor(x => x.EvaluationCriteriaId)
                .IsValidId();

            RuleFor(x => x.Comments)
                .NotEmpty().WithMessage("Comments are required")
                .IsValidDescription(500);

            RuleFor(x => x.Score)
                .IsValidScore();
        }
    }
}

