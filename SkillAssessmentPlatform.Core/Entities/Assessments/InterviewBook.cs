using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class InterviewBook
    {
        [Key]
        public int Id { get; set; }
        public int InterviewId { get; set; }
        public int AppointmentId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? MeetingLink { get; set; }
        public InterviewStatus Status { get; set; }
        public int? FeedbackId { get; set; }
        public int StageProgressId { get; set; }

        // Navigation properties
        public Interview Interview { get; set; }
        public Appointment Appointment { get; set; }
        public Feedback? Feedback { get; set; }
        public Applicant Applicant { get; set; }
        public StageProgress StageProgress { get; set; }


    }
}
