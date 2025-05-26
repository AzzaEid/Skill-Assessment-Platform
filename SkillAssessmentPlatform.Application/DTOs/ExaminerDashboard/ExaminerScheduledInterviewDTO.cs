using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerScheduledInterviewDTO
    {
        public int Id { get; set; }
        public string ApplicantId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string MeetingLink { get; set; }
        public InterviewStatus Status { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public int DurationMinutes { get; set; }
    }

}
