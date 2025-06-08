namespace SkillAssessmentPlatform.Application.DTOs
{
    public class EvaluationCriteriaDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public bool IsActive { get; set; }
    }

}
