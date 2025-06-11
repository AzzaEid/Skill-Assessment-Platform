namespace SkillAssessmentPlatform.Application.DTOs.Certificate.Output
{
    public class AppCertificateDTO
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public int LevelProgressId { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }
    }
}
