namespace SkillAssessmentPlatform.Core.Exceptions
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public string? AttemptedValue { get; set; }
    }
}
