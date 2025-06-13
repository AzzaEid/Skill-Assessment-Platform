using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Enrollment;

namespace SkillAssessmentPlatform.Application.Validators.Enrollment
{
    public class UpdateEnrollmentStatusDTOValidator : AbstractValidator<UpdateEnrollmentStatusDTO>
    {
        public UpdateEnrollmentStatusDTOValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid enrollment status");
        }
    }
}
