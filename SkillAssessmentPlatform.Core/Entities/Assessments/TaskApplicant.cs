using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews
{
    public class TaskApplicant
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string ApplicantId { get; set; }

        public DateTime AssignedDate { get; set; }

        public DateTime DueDate { get; set; }
        public int StageProgressId { get; set; }

        // Navigation properties
        public AppTask Task { get; set; }

        public Applicant Applicant { get; set; }

        public ICollection<TaskSubmission> TaskSubmissions { get; set; } = new List<TaskSubmission>();
        public StageProgress StageProgress { get; set; }
    }
}
