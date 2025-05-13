using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.InterviewBook
{
    public class InterviewBookDTO
    {
        public int Id { get; set; }
        public int InterviewId { get; set; }
        public int AppointmentId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string MeetingLink { get; set; }
        public InterviewStatus Status { get; set; }
        public string ExaminerName { get; set; }
        public int DurationMinutes { get; set; }
    }
}
