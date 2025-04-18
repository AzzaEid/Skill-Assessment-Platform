using SkillAssessmentPlatform.Core.Entities;
using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Core.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string SeniorExaminerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public string AssociatedSkills { get; set; }
        public bool IsActive { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation properties
        public Examiner SeniorExaminer { get; set; }
        public ICollection<Examiner> Examiners { get; set; }
        public ICollection<Level> Levels { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}


