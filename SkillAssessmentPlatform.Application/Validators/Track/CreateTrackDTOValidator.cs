using FluentValidation;
using Microsoft.AspNetCore.Http;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Application.Validators.Base;

namespace SkillAssessmentPlatform.Application.Validators.Track
{
    public class CreateTrackDTOValidator : AbstractValidator<CreateTrackDTO>
    {
        public CreateTrackDTOValidator()
        {
            RuleFor(x => x.Name)
                .IsValidTitle(100);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .IsValidDescription(1000);

            RuleFor(x => x.Objectives)
                .NotEmpty().WithMessage("Objectives are required")
                .IsValidDescription(2000);

            When(x => !string.IsNullOrEmpty(x.SeniorExaminerID), () =>
            {
                RuleFor(x => x.SeniorExaminerID)
                    .IsValidId();
            });

            RuleFor(x => x.ImageFile)
                .Must(BeValidImage)
                .WithMessage("Image must be a valid image file (jpg, jpeg, png, gif)")
                .When(x => x.ImageFile != null);

            RuleForEach(x => x.AssociatedSkills)
                .SetValidator(new CreateAssociatedSkillDTOValidator());
        }

        private bool BeValidImage(IFormFile file)
        {
            if (file == null) return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension) && file.Length > 0 && file.Length <= 5 * 1024 * 1024; // 5MB max
        }
    }

}
