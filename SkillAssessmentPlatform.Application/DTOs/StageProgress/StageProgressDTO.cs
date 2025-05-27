using SkillAssessmentPlatform.Core.Enums;
namespace SkillAssessmentPlatform.Application.DTOs.StageProgress
{
    public class StageProgressDTO
    {
        public int Id { get; set; }
        public int LevelProgressId { get; set; }
        public int StageId { get; set; }
        public string StageName { get; set; }
        public StageType StageType { get; set; }
        public ProgressStatus Status { get; set; }
        public int Score { get; set; }
        public string ApplicantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int Attempts { get; set; }
        public StageActionStatus ActionStatus { get; set; }
        public object AdditionalData { get; set; }
    }



}
