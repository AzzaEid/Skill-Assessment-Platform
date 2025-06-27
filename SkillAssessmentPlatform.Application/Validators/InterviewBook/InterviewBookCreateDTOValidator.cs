using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook.Input;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.InterviewBook
{
    public class InterviewBookCreateDTOValidator : AbstractValidator<InterviewBookCreateDTO>
    {
        public InterviewBookCreateDTOValidator()
        {
            RuleFor(x => x.InterviewId)
                .IsValidId();

            RuleFor(x => x.AppointmentId)
                .IsValidId();
        }
    }
}
