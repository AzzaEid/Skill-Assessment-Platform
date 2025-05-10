namespace SkillAssessmentPlatform.Core.Entities.Feedback_and_Evaluation
{
    public class EvaluationCriteria
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Weight { get; set; }
        public Boolean IsActive { get; set; } = true;
        // Navigation properties
        public Stage Stage { get; set; }
        public ICollection<DetailedFeedback> DetailedFeedbacks { get; set; } = new List<DetailedFeedback>();
    }
}
