using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.StageProgress.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.StageProgress
{
    public class AssignExaminerDTOValidator : AbstractValidator<AssignExaminerDTO>
    {
        public AssignExaminerDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();
        }
    }
}
