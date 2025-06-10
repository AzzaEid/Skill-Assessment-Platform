using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerInterviewRequestDTO
    {
        public int Id { get; set; }
        public int StageProgressId { get; set; }
        public int InterviewId { get; set; }
        public string ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public DateTime RequestDate { get; set; }
        public int DaysWaiting { get; set; }
        public InterviewStatus Status { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public int MaxDaysToBook { get; set; }
    }
}
