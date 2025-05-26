namespace SkillAssessmentPlatform.Core.Entities.TrackLevelStage
{
    public class AssociatedSkill
    {
        public int Id { get; set; }
        public int TrackId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public Track Track { get; set; }
    }
}
