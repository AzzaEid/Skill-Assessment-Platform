using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{

    public class ExaminerExamReviewDTO
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ApplicantId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int DaysWaiting { get; set; }
        public ExamRequestStatus Status { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public string Difficulty { get; set; }
    }


}
