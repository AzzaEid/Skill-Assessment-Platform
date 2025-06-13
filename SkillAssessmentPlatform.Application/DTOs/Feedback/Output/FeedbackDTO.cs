namespace SkillAssessmentPlatform.Application.DTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public string ExaminerId { get; set; }
        public string Comments { get; set; }
        public decimal TotalScore { get; set; }
        public DateTime FeedbackDate { get; set; }
        public ICollection<DetailedFeedbackDTO> DetailedFeedbacks { get; set; } = new List<DetailedFeedbackDTO>();
    }
}
