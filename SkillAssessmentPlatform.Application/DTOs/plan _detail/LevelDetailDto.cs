namespace SkillAssessmentPlatform.Application.DTOs
{
    public class LevelDetailDto
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public List<StageDetailDTO> Stages { get; set; }



    }
}
