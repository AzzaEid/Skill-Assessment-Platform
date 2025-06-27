namespace SkillAssessmentPlatform.Application.DTOs.Structure
{
    public class LevelStructureDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Order { get; set; }
        public List<StageStructureDTO?> Stages { get; set; }
    }
}
