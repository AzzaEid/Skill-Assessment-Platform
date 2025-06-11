using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class TaskSummaryDTO
    {
        public int? TaskApplicantId { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? TaskSubmissionId { get; set; }
        public TaskSubmissionStatus? SubmissionStatus { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string SubmissionUrl { get; set; }
        public bool HasFeedback { get; set; }
        public int? FeedbackId { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public string TaskRequirements { get; set; }
    }
}
