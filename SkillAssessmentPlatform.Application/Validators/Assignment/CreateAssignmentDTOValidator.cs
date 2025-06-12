using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.CreateAssignment;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Assignment
{
    public class CreateAssignmentDTOValidator : AbstractValidator<CreateAssignmentDTO>
    {
        public CreateAssignmentDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();

            RuleFor(x => x.StageId)
                .IsValidId();

            RuleFor(x => x.DueDate)
                .IsValidFutureDate();

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid creation type");

            RuleFor(x => x.AssignedBySeniorId)
                .IsValidId();

            When(x => !string.IsNullOrEmpty(x.Notes), () =>
            {
                RuleFor(x => x.Notes)
                    .IsValidDescription(1000);
            });
        }
    }
}
