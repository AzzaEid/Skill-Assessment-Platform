using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Exam.Input;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class CreateExamDetailsDtoValidator : AbstractValidator<CreateExamDetailsDto>
    {
        public CreateExamDetailsDtoValidator()
        {
            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes")
                .LessThanOrEqualTo(180).WithMessage("Duration must not exceed 180 minutes");

            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("Difficulty is required")
                .Must(BeValidDifficulty).WithMessage("Difficulty must be one of: Easy, Medium, Hard");

            RuleFor(x => x.QuestionsType)
                .IsInEnum().WithMessage("Invalid question type");
        }

        private bool BeValidDifficulty(string difficulty)
        {
            var allowed = new[] { "Easy", "Medium", "Hard" };
            return allowed.Contains(difficulty?.Trim(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
