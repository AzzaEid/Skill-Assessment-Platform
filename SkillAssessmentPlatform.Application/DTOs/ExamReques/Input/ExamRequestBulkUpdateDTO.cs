using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExamReques.Input
{
    public class ExamRequestBulkUpdateDTO
    {
        public List<int> RequestIds { get; set; }
        public ExamRequestStatus Status { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Instructions { get; set; }
    }
}
