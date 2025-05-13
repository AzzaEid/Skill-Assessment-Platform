using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Entities.Tasks__Exams__and_Interviews;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Users
{
    public class Applicant : User
    {
        public ApplicantStatus Status { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public ICollection<AppCertificate> Certificates { get; set; } = new HashSet<AppCertificate>();
        public ICollection<Applicant> Applicants { get; set; } = new HashSet<Applicant>();
        public ICollection<TaskApplicant> TaskApplicants { get; set; } = new HashSet<TaskApplicant>();
        public ICollection<ExamRequest> ExamRequests { get; set; } = new HashSet<ExamRequest>();
        public ICollection<InterviewBook> InterviewBooks { get; set; } = new HashSet<InterviewBook>();


    }
}
