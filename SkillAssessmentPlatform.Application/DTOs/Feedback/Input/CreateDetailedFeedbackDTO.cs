namespace SkillAssessmentPlatform.Application.DTOs
{
    public class CreateDetailedFeedbackDTO
    {
        //  public int FeedbackId { get; set; }
        public int EvaluationCriteriaId { get; set; }
        public string Comments { get; set; }
        public decimal Score { get; set; }
    }

}
