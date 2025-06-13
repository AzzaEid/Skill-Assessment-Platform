namespace SkillAssessmentPlatform.Application.DTOs.EvaluationCriteria.Input
{
    public class CreateEvaluationCriteriaDto
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
    }
}
