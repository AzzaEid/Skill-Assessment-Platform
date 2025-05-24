using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExamReques
{
    public class ExamRequestUpdateDTO
    {
        public ExamRequestStatus Status { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Instructions { get; set; }
    }
}
