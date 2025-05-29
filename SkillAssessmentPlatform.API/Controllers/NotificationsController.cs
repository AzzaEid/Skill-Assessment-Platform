using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillAssessmentPlatform.API.Common;
using SkillAssessmentPlatform.Application.Services;
using System.Security.Claims;

namespace SkillAssessmentPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly IResponseHandler _responseHandler;

        public NotificationsController(NotificationService notificationService, IResponseHandler responseHandler)
        {
            _notificationService = notificationService;
            _responseHandler = responseHandler;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return _responseHandler.Unauthorized();
            var result = _notificationService.GetByUserId(userId);
            return _responseHandler.Success(result);
        }
    }
}
