using FluentValidation;

namespace SkillAssessmentPlatform.Application.Validators.Base
{
    public static class CommonValidators
    {
        public static IRuleBuilder<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
        }

        public static IRuleBuilder<T, string> IsValidPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");
        }

        public static IRuleBuilder<T, string> IsValidFullName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Full name is required")
                .MinimumLength(2).WithMessage("Full name must be at least 2 characters long")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters")
                .Matches(@"^[a-zA-Z\s\-'\.]+$").WithMessage("Full name can only contain letters, spaces, hyphens, apostrophes, and dots");
        }

        public static IRuleBuilder<T, string> IsValidId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("ID is required")
                .Length(36).WithMessage("ID must be a valid GUID format");
        }

        public static IRuleBuilder<T, int> IsValidId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage("ID must be greater than 0");
        }

        public static IRuleBuilder<T, string> IsValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Invalid URL format");
        }

        public static IRuleBuilder<T, DateTime> IsValidFutureDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(DateTime.Now).WithMessage("Date must be in the future");
        }

        public static IRuleBuilder<T, DateTime?> IsValidFutureDate<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
        {
            return ruleBuilder
                .Must(date => !date.HasValue || date.Value > DateTime.Now)
                .WithMessage("Date must be in the future");
        }

        public static IRuleBuilder<T, decimal> IsValidScore<T>(this IRuleBuilder<T, decimal> ruleBuilder,
            decimal minScore = 0, decimal maxScore = 100)
        {
            return ruleBuilder
                .InclusiveBetween(minScore, maxScore)
                .WithMessage($"Score must be between {minScore} and {maxScore}");
        }

        public static IRuleBuilder<T, float> IsValidWeight<T>(this IRuleBuilder<T, float> ruleBuilder)
        {
            return ruleBuilder
                .InclusiveBetween(0.01f, 1.0f)
                .WithMessage("Weight must be between 0.01 and 1.0");
        }

        public static IRuleBuilder<T, string> IsValidDescription<T>(this IRuleBuilder<T, string> ruleBuilder,
            int maxLength = 500)
        {
            return ruleBuilder
                .MaximumLength(maxLength).WithMessage($"Description must not exceed {maxLength} characters");
        }

        public static IRuleBuilder<T, string> IsValidTitle<T>(this IRuleBuilder<T, string> ruleBuilder,
            int maxLength = 200)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(maxLength).WithMessage($"Title must not exceed {maxLength} characters");
        }
    }
}
