using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class ExamRequest
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Instructions { get; set; } = " ";
        public int? FeedbackId { get; set; }
        public ExamRequestStatus Status { get; set; }
        public int StageProgressId { get; set; }

        // Navigation properties
        public Exam Exam { get; set; }
        public Feedback Feedback { get; set; }
        public Applicant Applicant { get; set; }
        public StageProgress StageProgress { get; set; }

    }

}

