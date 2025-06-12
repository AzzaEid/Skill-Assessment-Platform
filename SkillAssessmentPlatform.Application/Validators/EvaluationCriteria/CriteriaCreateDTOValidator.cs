using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Level.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.EvaluationCriteria
{
    public class CriteriaCreateDTOValidator : AbstractValidator<CriteriaCreateDTO>
    {
        public CriteriaCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(500);

            RuleFor(x => x.Weight)
                .IsValidWeight();

            RuleFor(x => x.TrackId)
                .IsValidId();

            RuleFor(x => x.TrackName)
                .NotEmpty().WithMessage("Track name is required")
                .MaximumLength(100).WithMessage("Track name must not exceed 100 characters");
        }
    }


}
