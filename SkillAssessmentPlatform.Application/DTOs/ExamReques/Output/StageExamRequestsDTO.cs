namespace SkillAssessmentPlatform.Application.DTOs.ExamReques.Output
{
    public class StageExamRequestsDTO
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public string LevelName { get; set; }
        public int PendingRequestsCount { get; set; }
    }
}
