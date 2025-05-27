using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillAssessmentPlatform.Core.Entities.Management
{
    public class CreationAssignment
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public int StageId { get; set; }
        public CreationType Type { get; set; } // Task or Exam
        public AssignmentStatus Status { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public string AssignedBySeniorId { get; set; } // Senior Examiner who assigned 
        public string? Notes { get; set; }

        // تتبع انشاء الامتحان او التاسك
        //public bool? IsCreated { get; set; } = false; 
        // Navigation Properties
        [ForeignKey(nameof(ExaminerId))]
        public Examiner Examiner { get; set; }

        [ForeignKey(nameof(AssignedBySeniorId))]
        public Examiner AssignedBySenior { get; set; }
        public Stage Stage { get; set; }
    }
}
