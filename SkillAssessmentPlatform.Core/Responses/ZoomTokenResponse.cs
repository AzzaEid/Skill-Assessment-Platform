using Newtonsoft.Json;

namespace SkillAssessmentPlatform.Core.Responses
{
    public class ZoomTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

    }

}
