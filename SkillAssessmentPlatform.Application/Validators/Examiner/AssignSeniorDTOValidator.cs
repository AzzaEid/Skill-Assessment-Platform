using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Examiner
{
    public class AssignSeniorDTOValidator : AbstractValidator<AssignSeniorDTO>
    {
        public AssignSeniorDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();

            RuleFor(x => x.TrackId)
                .IsValidId();
        }
    }
}
