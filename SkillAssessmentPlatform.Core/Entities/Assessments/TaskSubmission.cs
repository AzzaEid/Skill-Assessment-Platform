using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class TaskSubmission
    {
        [Key]
        public int Id { get; set; }
        public int TaskApplicantId { get; set; }
        public string SubmissionUrl { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int? FeedbackId { get; set; }
        public TaskSubmissionStatus Status { get; set; }
        // Navigation properties
        public TaskApplicant TaskApplicant { get; set; }
        public Feedback? Feedback { get; set; }

    }
}
