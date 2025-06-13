using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Task
{
    public class UpdateAppTaskDtoValidator : AbstractValidator<UpdateAppTaskDto>
    {
        public UpdateAppTaskDtoValidator()
        {
            RuleFor(x => x.Id)
                .IsValidId();

            Include(new CreateAppTaskDtoValidator());
        }
    }
}
