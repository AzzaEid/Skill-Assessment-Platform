using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;

namespace SkillAssessmentPlatform.Application.Validators.Examiner
{
    public class UpdateWorkLoadDTOValidator : AbstractValidator<UpdateWorkLoadDTO>
    {
        public UpdateWorkLoadDTOValidator()
        {
            RuleFor(x => x.MaxWorkLoad)
                .InclusiveBetween(1, 100)
                .WithMessage("Max workload must be between 1 and 100");
        }
    }
}
