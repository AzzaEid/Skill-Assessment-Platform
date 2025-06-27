using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Interview
{
    public class CreateInterviewDetailsDtoValidator : AbstractValidator<CreateInterviewDetailsDto>
    {
        public CreateInterviewDetailsDtoValidator()
        {
            RuleFor(x => x.MaxDaysToBook)
                .InclusiveBetween(1, 30)
                .WithMessage("Max days to book must be between 1 and 30");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(15, 240)
                .WithMessage("Interview duration must be between 15 minutes and 4 hours");

            When(x => !string.IsNullOrEmpty(x.Instructions), () =>
            {
                RuleFor(x => x.Instructions)
                    .IsValidDescription(1000);
            });
        }
    }
}
