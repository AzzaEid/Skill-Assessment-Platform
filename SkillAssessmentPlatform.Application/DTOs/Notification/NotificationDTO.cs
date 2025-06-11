using SkillAssessmentPlatform.Core.Enums;

namespace SkillAssessmentPlatform.Application.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }

    }
}
