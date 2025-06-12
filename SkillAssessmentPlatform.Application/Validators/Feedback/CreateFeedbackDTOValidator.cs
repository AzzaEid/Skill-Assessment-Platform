using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Feedback
{
    public class CreateFeedbackDTOValidator : AbstractValidator<CreateFeedbackDTO>
    {
        public CreateFeedbackDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();

            RuleFor(x => x.Comments)
                .NotEmpty().WithMessage("Comments are required")
                .IsValidDescription(1000);

            RuleFor(x => x.TotalScore)
                .IsValidScore();

            RuleFor(x => x.StageProgressId)
                .IsValidId();

            RuleFor(x => x.ResultStatus)
                .IsInEnum().WithMessage("Invalid result status");

            RuleFor(x => x.DetailedFeedbacks)
                .NotEmpty().WithMessage("Detailed feedbacks are required");

            RuleForEach(x => x.DetailedFeedbacks)
                .SetValidator(new CreateDetailedFeedbackDTOValidator());

            // At least one of these should be provided
            RuleFor(x => x)
                .Must(x => x.TaskSubmissionId.HasValue || x.ExamRequestId.HasValue || x.InterviewBookId.HasValue)
                .WithMessage("At least one reference ID (Task, Exam, or Interview) must be provided");
        }
    }

}
