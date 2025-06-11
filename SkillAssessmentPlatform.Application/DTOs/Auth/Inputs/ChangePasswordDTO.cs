namespace SkillAssessmentPlatform.Application.DTOs.Auth.Inputs
{
    public class ChangePasswordDTO
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
