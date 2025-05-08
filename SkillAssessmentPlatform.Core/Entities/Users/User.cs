using Microsoft.AspNetCore.Identity;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Users
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Actors UserType { get; set; }
        public string? Image { get; set; }
        public Gender? Gender { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}
