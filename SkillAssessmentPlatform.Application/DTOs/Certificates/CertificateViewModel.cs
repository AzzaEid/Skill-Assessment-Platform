namespace SkillAssessmentPlatform.Application.DTOs.Certificates
{
    public class CertificateViewModel
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public string TrackName { get; set; }
        public string LevelName { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }

    }
}
