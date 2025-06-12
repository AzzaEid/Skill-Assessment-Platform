using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Task
{
    public class CreateTaskSubmissionDTOValidator : AbstractValidator<CreateTaskSubmissionDTO>
    {
        public CreateTaskSubmissionDTOValidator()
        {
            RuleFor(x => x.TaskApplicantId)
                .IsValidId();

            RuleFor(x => x.SubmissionUrl)
                .NotEmpty().WithMessage("Submission URL is required")
                .IsValidUrl();
        }
    }
}
