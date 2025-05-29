using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.CreateAssignment
{
    public class CreationAssignmentDTO
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public string ExaminerName { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string TrackName { get; set; }
        public CreationType Type { get; set; }
        public AssignmentStatus Status { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysRemaining => (DueDate - DateTime.Now).Days;
        public string AssignedBySeniorName { get; set; }
        public string? Notes { get; set; }

        public TasksPoolDto TasksPool { get; set; }
        public ExamDto Exam { get; set; }

    }
}