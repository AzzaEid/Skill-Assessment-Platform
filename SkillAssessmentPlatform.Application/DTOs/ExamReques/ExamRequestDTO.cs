using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExamReques
{
    public class ExamRequestDTO
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public int ExamId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Instructions { get; set; }
        public ExamRequestStatus Status { get; set; }
        public ExamDto Exam { get; set; }
        public ApplicantDTO Applicant { get; set; }
        // public FeedbackDTO Feedback { get; set; }
    }
}
