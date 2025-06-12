using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Enrollment;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Enrollment
{
    public class EnrollmentCreateDTOValidator : AbstractValidator<EnrollmentCreateDTO>
    {
        public EnrollmentCreateDTOValidator()
        {
            RuleFor(x => x.TrackId)
                .IsValidId();
        }
    }
}
