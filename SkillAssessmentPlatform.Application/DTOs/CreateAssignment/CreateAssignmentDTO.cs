using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs.CreateAssignment
{

    public class CreateAssignmentDTO
    {
        public string ExaminerId { get; set; }
        public int StageId { get; set; }
        public DateTime DueDate { get; set; }
        public CreationType Type { get; set; }
        public string AssignedBySeniorId { get; set; }
        //  public int RequiredTasksCount { get; set; }
        public string? Notes { get; set; }
    }
}
