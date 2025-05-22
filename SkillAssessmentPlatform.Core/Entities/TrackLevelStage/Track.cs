using SkillAssessmentPlatform.Core.Entities.TrackLevelStage;
using SkillAssessmentPlatform.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage;
using SkillAssessmentPlatform.Core.Entities.TrackLevelStage.SkillAssessmentPlatform.Core.Entities;


namespace SkillAssessmentPlatform.Core.Entities
{
    public class Track
    {
        public int Id { get; set; }

        public string? SeniorExaminerID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Objectives { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Image { get; set; }

        public DateTime? CreatedAt { get; set; }

        // ✅ Navigation properties
        public Examiner? SeniorExaminer { get; set; }

        public ICollection<Examiner> Examiners { get; set; }

        public ICollection<Level> Levels { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<AssociatedSkill> AssociatedSkills { get; set; } = new List<AssociatedSkill>();
    }
}
