using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications
{
    public class AppCertificate
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int LeveProgressId { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }
        // Navigation properties
        public Applicant Applicant { get; set; }
        public LevelProgress LevelProgress { get; set; }
    }
}
