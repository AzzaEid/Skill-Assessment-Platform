namespace SkillAssessmentPlatform.Application.DTOs
{
    public class DetailedFeedbackDTO
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int EvaluationCriteriaId { get; set; }
        public string CriterionName { get; set; }
        public float CriterionWeight { get; set; }
        public string Comments { get; set; }
        public decimal Score { get; set; }
    }

}
