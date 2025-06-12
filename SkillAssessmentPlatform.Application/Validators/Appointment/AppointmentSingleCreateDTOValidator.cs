using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Appointment
{
    public class AppointmentSingleCreateDTOValidator : AbstractValidator<AppointmentSingleCreateDTO>
    {
        public AppointmentSingleCreateDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();

            RuleFor(x => x.StartTime)
                .IsValidFutureDate();

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("End time must be after start time");
        }
    }
}

