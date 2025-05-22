namespace SkillAssessmentPlatform.Application.DTOs.ExamReques
{
    public class PendingExamRequestSummaryDTO
    {
        public List<StageExamRequestsDTO> Stages { get; set; } = new List<StageExamRequestsDTO>();
    }
}
