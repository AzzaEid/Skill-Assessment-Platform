using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;

namespace SkillAssessmentPlatform.Core.Interfaces.Repository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        public Task<IEnumerable<Notification>> GetByUserId(string userId);
    }
}
