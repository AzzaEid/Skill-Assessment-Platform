namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TaskSubmissionDTO
    {
        public int Id { get; set; }
        public int TaskApplicantId { get; set; }
        public string SubmissionUrl { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

}
