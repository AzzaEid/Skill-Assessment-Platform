using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Track
{
    public class ExaminerAssignmentDtoValidator : AbstractValidator<ExaminerAssignmentDto>
    {
        public ExaminerAssignmentDtoValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();
        }
    }
}
