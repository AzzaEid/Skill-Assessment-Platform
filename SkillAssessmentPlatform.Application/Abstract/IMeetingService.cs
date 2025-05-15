using SkillAssessmentPlatform.Core.Responses;

namespace SkillAssessmentPlatform.Application.Abstract
{
    public interface IMeetingService
    {
        Task<string> GetAccessTokenAsync();
        Task<ZoomMeetingResponse> CreateMeetingAsync(DateTime startTime, DateTime endTime, string topic);
        Task<string> CreateMeetingAsync(DateTime startTime, DateTime endTime, string hostId, string participantId, string title);
    }
}
