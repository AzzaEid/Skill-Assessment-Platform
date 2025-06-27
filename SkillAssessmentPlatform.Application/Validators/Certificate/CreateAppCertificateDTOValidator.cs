using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Certificate.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Certificate
{
    public class CreateAppCertificateDTOValidator : AbstractValidator<CreateAppCertificateDTO>
    {
        public CreateAppCertificateDTOValidator()
        {
            RuleFor(x => x.ApplicantId)
                .IsValidId();

            RuleFor(x => x.LevelProgressId)
                .IsValidId();
        }
    }
}
