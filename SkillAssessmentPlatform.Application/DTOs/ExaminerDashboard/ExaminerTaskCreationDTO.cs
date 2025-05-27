namespace SkillAssessmentPlatform.Application.DTOs.ExaminerDashboard
{
    public class ExaminerTaskCreationDTO
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public int RequiredTasksCount { get; set; }
        public int CreatedTasksCount { get; set; }
        public DateTime AssignedDate { get; set; }
        public int DaysRemaining { get; set; }
    }
}
