using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.ExamReques.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.ExamRequest
{
    public class ExamRequestUpdateDTOValidator : AbstractValidator<ExamRequestUpdateDTO>
    {
        public ExamRequestUpdateDTOValidator()
        {
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
