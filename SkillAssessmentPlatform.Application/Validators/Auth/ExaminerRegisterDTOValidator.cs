using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Auth.Inputs;

namespace SkillAssessmentPlatform.Application.Validators.Auth
{
    public class ExaminerRegisterDTOValidator : AbstractValidator<ExaminerRegisterDTO>
    {
        public ExaminerRegisterDTOValidator()
        {
            Include(new UserRegisterDTOValidator());

            RuleFor(x => x.WorkingTrackIds)
                .NotEmpty().WithMessage("At least one working track must be selected")
                .Must(ids => ids.All(id => id > 0))
                .WithMessage("All track IDs must be valid");
        }
    }
}
