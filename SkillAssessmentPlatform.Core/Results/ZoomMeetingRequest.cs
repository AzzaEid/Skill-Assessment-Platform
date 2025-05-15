namespace SkillAssessmentPlatform.Core.Results
{
    public class ZoomMeetingRequest
    {
        public string Topic { get; set; }
        public int Type { get; set; } = 2; // Scheduled meeting
        public DateTime StartTime { get; set; }
        public int Duration { get; set; } // in minutes
        public string Timezone { get; set; } = "UTC";
        public ZoomMeetingSettings Settings { get; set; } = new();
    }

}
