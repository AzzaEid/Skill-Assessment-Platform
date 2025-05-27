using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateFeedbackDTO
    {
        public string ExaminerId { get; set; }
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
        public int? TaskSubmissionId { get; set; }
        public int? ExamRequestId { get; set; }
        public int? InterviewBookId { get; set; }
        public int StageProgressId { get; set; }
        public ApplicantResultStatus ResultStatus { get; set; }
        public List<CreateDetailedFeedbackDTO> DetailedFeedbacks { get; set; }
    }

}
