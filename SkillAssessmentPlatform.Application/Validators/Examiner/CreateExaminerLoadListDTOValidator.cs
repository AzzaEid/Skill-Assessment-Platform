using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Examiner.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Examiner
{
    public class CreateExaminerLoadListDTOValidator : AbstractValidator<CreateExaminerLoadListDTO>
    {
        public CreateExaminerLoadListDTOValidator()
        {
            RuleFor(x => x.ExaminerID)
                .IsValidId();

            RuleFor(x => x.examinerLoads)
                .NotEmpty().WithMessage("At least one examiner load must be provided");

            RuleForEach(x => x.examinerLoads)
                .SetValidator(new CreateExaminerLoadDTOValidator());
        }
    }
}
