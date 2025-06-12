using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.ExamRequest
{
    public class ExamRequestBulkUpdateDTOValidator : AbstractValidator<ExamRequestBulkUpdateDTO>
    {
        public ExamRequestBulkUpdateDTOValidator()
        {
            RuleFor(x => x.RequestIds)
                .NotEmpty().WithMessage("At least one request ID must be provided")
                .Must(ids => ids.All(id => id > 0))
                .WithMessage("All request IDs must be valid");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid exam request status");

            When(x => x.ScheduledDate.HasValue, () =>
            {
                RuleFor(x => x.ScheduledDate)
                    .IsValidFutureDate();
            });

            When(x => !string.IsNullOrEmpty(x.Instructions), () =>
            {
                RuleFor(x => x.Instructions)
               .IsValidDescription(1000);
            });
        }
    }
}
