using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExamReques.Output
{
    public class ExamRequestInfoApplicantDTO
    {
        public int? ExamRequestId { get; set; }
        public ExamRequestStatus? RequestStatus { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Instructions { get; set; }


    }
}
