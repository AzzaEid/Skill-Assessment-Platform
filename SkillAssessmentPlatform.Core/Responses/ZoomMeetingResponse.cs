using Newtonsoft.Json;

namespace SkillAssessmentPlatform.Core.Responses
{
    public class ZoomMeetingResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("join_url")]
        public string JoinUrl { get; set; }

        [JsonProperty("start_url")]
        public string StartUrl { get; set; }
    }
}

