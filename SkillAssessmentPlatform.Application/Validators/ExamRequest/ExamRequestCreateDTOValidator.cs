using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.ExamRequest
{
    public class ExamRequestCreateDTOValidator : AbstractValidator<ExamRequestCreateDTO>
    {
        public ExamRequestCreateDTOValidator()
        {
            RuleFor(x => x.StageId)
                .IsValidId();

            RuleFor(x => x.ApplicantId)
                .IsValidId();
        }
    }
}
