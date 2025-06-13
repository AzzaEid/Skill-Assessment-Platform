namespace SkillAssessmentPlatform.Application.DTOs.Auth.Inputs
{
    public class ExaminerRegisterDTO : UserRegisterDTO
    {
        public List<int> WorkingTrackIds { get; set; }
    }
}
