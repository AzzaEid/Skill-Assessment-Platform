namespace SkillAssessmentPlatform.Application.DTOs.Auth
{
    public class ExaminerRegisterDTO : UserRegisterDTO
    {
        public List<int> WorkingTrackIds { get; set; }
    }
}
