using SkillAssessmentPlatform.Core.Entities.Users;

namespace SkillAssessmentPlatform.Application.DTOs.Auth.Output
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Actors UserType { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
    }
}
