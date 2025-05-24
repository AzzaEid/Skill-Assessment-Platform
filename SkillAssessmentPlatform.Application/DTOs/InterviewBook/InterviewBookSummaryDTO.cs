using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.InterviewBook
{
    public class InterviewBookSummaryDTO
    {

        public int? InterviewBookId { get; set; }
        public InterviewStatus? BookingStatus { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string MeetingLink { get; set; }
        public string Instructions { get; set; }
        public bool HasFeedback { get; set; }
        public int? FeedbackId { get; set; }
        public int MaxDaysToBook { get; set; }
    }
}

