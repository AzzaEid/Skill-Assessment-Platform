using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Interfaces.Repository;
using SkillAssessmentPlatform.Infrastructure.Data;

namespace SkillAssessmentPlatform.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly ILogger<NotificationRepository> _logger;

        public NotificationRepository(AppDbContext context, ILogger<NotificationRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Notification>> GetByUserId(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }
    }
}
