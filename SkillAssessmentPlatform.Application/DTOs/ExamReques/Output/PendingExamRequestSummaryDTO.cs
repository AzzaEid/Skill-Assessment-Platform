namespace SkillAssessmentPlatform.Application.DTOs.ExamReques.Output
{
    public class PendingExamRequestSummaryDTO
    {
        public List<StageExamRequestsDTO> Stages { get; set; } = new List<StageExamRequestsDTO>();
    }
}
