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

            RuleFor(x => x.Bio)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(250).WithMessage("Bio must not exceed 250 characters");
        }
    }
}
