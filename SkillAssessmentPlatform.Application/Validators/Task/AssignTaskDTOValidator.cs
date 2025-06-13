using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Task
{
    public class AssignTaskDTOValidator : AbstractValidator<AssignTaskDTO>
    {
        public AssignTaskDTOValidator()
        {
            RuleFor(x => x.StageProgressId)
                .IsValidId();
        }
    }

}
