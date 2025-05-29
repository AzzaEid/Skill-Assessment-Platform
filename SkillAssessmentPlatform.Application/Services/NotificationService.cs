using AutoMapper;
using Microsoft.Extensions.Logging;
using SkillAssessmentPlatform.Application.DTOs;
using SkillAssessmentPlatform.Core.Entities.Certificates_and_Notifications;
using SkillAssessmentPlatform.Core.Enums;
using SkillAssessmentPlatform.Core.Interfaces;

namespace SkillAssessmentPlatform.Application.Services
{
    public class NotificationService
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(ILogger<NotificationService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SendNotificationAsync(string userId, NotificationType title, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Message = message,
                    Type = title,
                    Date = DateTime.Now,
                    IsRead = false
                };

                await _unitOfWork.NotificationRepository.AddAsync(notification);

                _logger.LogInformation($"Notification sent to user {userId}: {title}");

                // TODO:: SignalR 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send notification to user {userId}");
                throw;
            }
        }
        public async Task<IEnumerable<NotificationDTO>> GetByUserId(string userId)
        {
            var list = await _unitOfWork.NotificationRepository.GetByUserId(userId);
            return _mapper.Map<List<NotificationDTO>>(list);
        }
    }
}
