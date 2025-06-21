namespace SkillAssessmentPlatform.Application.DTOs.ExamReques.Input
{
    public class ExamRequestCreateDTO
    {
        public int StageId { get; set; }
        public string ApplicantId { get; set; }
        public string Instructions { get; set; }
        public int StageProgressId { get; set; }
    }

}
