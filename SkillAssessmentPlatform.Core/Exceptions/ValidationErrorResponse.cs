namespace SkillAssessmentPlatform.Core.Exceptions
{
    internal class ValidationErrorResponse
    {
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; } = new();
    }
}
