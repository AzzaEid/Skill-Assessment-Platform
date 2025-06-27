namespace SkillAssessmentPlatform.Application.DTOs.Certificate.Output
{
    public class VerifyCertificateResultDTO
    {
        public string ApplicantFullName { get; set; }
        public string TrackName { get; set; }
        public string LevelName { get; set; }
        public DateTime IssueDate { get; set; }
        public string VerificationCode { get; set; }
    }

}
