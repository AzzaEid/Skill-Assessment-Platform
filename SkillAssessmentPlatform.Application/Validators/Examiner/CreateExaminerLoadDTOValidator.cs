using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;

namespace SkillAssessmentPlatform.Application.Validators.Examiner
{
    public class CreateExaminerLoadDTOValidator : AbstractValidator<CreateExaminerLoadDTO>
    {
        public CreateExaminerLoadDTOValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid load type");

            RuleFor(x => x.MaxWorkLoad)
                .InclusiveBetween(1, 100)
                .WithMessage("Max workload must be between 1 and 100");
        }
    }
}
