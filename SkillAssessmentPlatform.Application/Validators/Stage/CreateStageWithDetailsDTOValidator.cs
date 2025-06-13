using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;
using SkillAssessmentPlatform.Application.Validators.EvaluationCriteria;
using SkillAssessmentPlatform.Application.Validators.Interview;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.Validators.Stage
{
    public class CreateStageWithDetailsDTOValidator : AbstractValidator<CreateStageWithDetailsDTO>
    {
        public CreateStageWithDetailsDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .IsValidDescription(1000);

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid stage type");

            RuleFor(x => x.Order)
                .GreaterThan(0).WithMessage("Order must be greater than 0");

            RuleFor(x => x.PassingScore)
                .InclusiveBetween(0, 100)
                .WithMessage("Passing score must be between 0 and 100");

            RuleFor(x => x.EvaluationCriteria)
                .NotEmpty().WithMessage("At least one evaluation criteria must be provided");

            RuleForEach(x => x.EvaluationCriteria)
                .SetValidator(new EvaluationCriteriaDTOValidator());

            // Conditional validation based on stage type
            When(x => x.Type == StageType.Exam, () =>
            {
                RuleFor(x => x.Exam)
                    .NotNull().WithMessage("Exam details are required for exam stages")
                    .SetValidator(new CreateExamDetailsDtoValidator());
            });

            When(x => x.Type == StageType.Interview, () =>
            {
                RuleFor(x => x.Interview)
                    .NotNull().WithMessage("Interview details are required for interview stages")
                    .SetValidator(new CreateInterviewDetailsDtoValidator());
            });

            When(x => x.Type == StageType.Task, () =>
            {
                RuleFor(x => x.TasksPool)
                    .NotNull().WithMessage("Tasks pool details are required for task stages")
                    .SetValidator(new CreateTasksPoolDetailsDtoValidator());
            });
        }
    }
}
