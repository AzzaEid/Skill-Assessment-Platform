using SkillAssessmentPlatform.Application.DTOs.ExamReques.Output;
using SkillAssessmentPlatform.Application.DTOs.InterviewBook.Output;

namespace SkillAssessmentPlatform.Application.DTOs.Feedback.Output
{
    public class FeedbackWithInfoDTO
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
        public DateTime FeedbackDate { get; set; }
        public ICollection<DetailedFeedbackDTO> DetailedFeedbacks { get; set; } = new List<DetailedFeedbackDTO>();
        public TaskSubmissionDTO TaskSubmission { get; set; }
        public InterviewBookSummaryDTO InterviewBook { get; set; }
        public ExamRequestInfoDTO ExamRequest { get; set; }
    }
}
