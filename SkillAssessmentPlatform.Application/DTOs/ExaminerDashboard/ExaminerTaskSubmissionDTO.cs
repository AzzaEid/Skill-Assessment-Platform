using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerTaskSubmissionDTO
    {
        public int Id { get; set; }
        public int StageProgressId { get; set; }
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public string ApplicantId { get; set; }
        public string SubmissionUrl { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysWaiting { get; set; }
        public bool IsLate { get; set; }
        public TaskSubmissionStatus Status { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
    }
}
