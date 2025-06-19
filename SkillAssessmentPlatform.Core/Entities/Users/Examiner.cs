using SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation;
using SkillAssessmentPlatform.Core.Entities.Management;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;

namespace SkillAssessmentPlatform.Core.Entities.Users
{
    public class Examiner : User
    {
        public string Bio { get; set; }

        public ICollection<ExaminerLoad> ExaminerLoads { get; set; }
        public ICollection<Track> ManagedTracks { get; set; } = new HashSet<Track>();
        public ICollection<Track> WorkingTracks { get; set; } = new List<Track>();
        public ICollection<StageProgress> SupervisedStages { get; set; } = new HashSet<StageProgress>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
        public ICollection<Feedback> Feedbacks { get; set; } = new HashSet<Feedback>();
        public ICollection<CreationAssignment> AssignedCreations { get; set; }
        public ICollection<CreationAssignment> CreatedAssignmentsAsSenior { get; set; }
    }

}

