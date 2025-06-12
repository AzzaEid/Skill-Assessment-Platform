using FluentValidation;
using SkillAssessmentPlatform.Application.DTOs.Appointment.Inputs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Appointment
{
    public class AppointmentBulkCreateDTOValidator : AbstractValidator<AppointmentBulkCreateDTO>
    {
        public AppointmentBulkCreateDTOValidator()
        {
            RuleFor(x => x.ExaminerId)
                .IsValidId();

            RuleFor(x => x.StartDate)
                .IsValidFutureDate();

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be after start date");

            RuleFor(x => x.SlotDurationMinutes)
                .InclusiveBetween(15, 120)
                .WithMessage("Slot duration must be between 15 and 120 minutes");

            RuleFor(x => x.StartHour)
                .InclusiveBetween(8, 22)
                .WithMessage("Start hour must be between 8 and 22");

            RuleFor(x => x.EndHour)
                .InclusiveBetween(8, 22)
                .WithMessage("End hour must be between 8 and 22")
                .GreaterThan(x => x.StartHour)
                .WithMessage("End hour must be after start hour");
        }
    }

}
