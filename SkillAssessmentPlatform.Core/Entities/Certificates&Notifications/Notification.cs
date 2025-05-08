using SkillAssessmentPlatform.Core.Entities.Users;
using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        // Navigation properties
        public User User { get; set; }
    }
}
