namespace SkillAssessmentPlatform.Application.Abstract
{
    public interface IMeetingService
    {
        Task<string> CreateMeetingAsync(DateTime startTime, DateTime endTime, string hostId, string participantId, string title);
    }
}
