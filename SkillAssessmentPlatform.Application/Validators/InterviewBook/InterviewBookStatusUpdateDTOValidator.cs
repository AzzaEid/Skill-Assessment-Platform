using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.Validators.InterviewBook
{
    public class InterviewBookStatusUpdateDTOValidator : AbstractValidator<InterviewBookStatusUpdateDTO>
    {
        public InterviewBookStatusUpdateDTOValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid interview status")
                .Must(status => new[] { InterviewStatus.Scheduled, InterviewStatus.Completed, InterviewStatus.Canceled }
                    .Contains(status))
                .WithMessage("Status must be one of: Scheduled, Completed, Canceled");
        }
    }
}
