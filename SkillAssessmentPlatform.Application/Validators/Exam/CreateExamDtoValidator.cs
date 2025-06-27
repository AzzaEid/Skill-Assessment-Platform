using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Exam.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Exam
{
    public class CreateExamDtoValidator : AbstractValidator<CreateExamDto>
    {
        public CreateExamDtoValidator()
        {
            RuleFor(x => x.StageId)
                .IsValidId();

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(15, 480)
                .WithMessage("Exam duration must be between 15 minutes and 8 hours");

            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("Difficulty level is required")
                .Must(d => new[] { "Easy", "Medium", "Hard" }.Contains(d))
                .WithMessage("Difficulty must be Easy, Medium, or Hard");
            /*
            RuleFor(x => x.QuestionsType)
                .NotEmpty().WithMessage("At least one question type must be selected")
                .Must(types => types.All(t => !string.IsNullOrWhiteSpace(t)))
                .WithMessage("Question types cannot be empty");
            /*/
        }
    }
}
