using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.Validators.Auth;

namespace SkillAssessmentPlatform.Application.Validators.Examiner
{
    public class UpdateExaminerDTOValidator : AbstractValidator<UpdateExaminerDTO>
    {
        public UpdateExaminerDTOValidator()
        {
            Include(new UpdateUserDTOValidator());

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");
        }
    }
}
