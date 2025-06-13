namespace SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation
{
    public class DetailedFeedback
    {
        public int Id { get; set; }
        public int FeedbackId { get; set; }
        public int CriterionId { get; set; }
        public string Comments { get; set; }
        public decimal Score { get; set; }
        // Navigation properties
        public Feedback Feedback { get; set; }
        public EvaluationCriteria EvaluationCriteria { get; set; }
    }
}
