using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;
using SkillAssessmentPlatform.Application.Validators.EvaluationCriteria;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class UpdateStageCriteriaPayloadDtoValidator : AbstractValidator<UpdateStageCriteriaPayloadDto>
    {
        public UpdateStageCriteriaPayloadDtoValidator()
        {
            RuleFor(x => x.StageId)
                .IsValidId();

            RuleForEach(x => x.UpdatedCriteria)
                .SetValidator(new UpdateEvaluationCriteriaDtoValidator())
                .When(x => x.UpdatedCriteria != null);

            RuleForEach(x => x.NewCriteriaToAdd)
                .SetValidator(new CreateEvaluationCriteriaDtoValidator())
                .When(x => x.NewCriteriaToAdd != null);



            RuleFor(x => x.DeletionMode)
                .IsInEnum().WithMessage("Invalid deletion handling mode")
                .When(x => x.DeletionMode.HasValue);
        }
    }
}
