using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Core.Responses;
using SkillAssessmentPlatform.Core.Results;
using SkillAssessmentPlatform.Infrastructure.ExternalServices;
using System.Net.Http.Headers;
using System.Text;

namespace SkillAssessmentPlatform.Application.Services
{
    public class ZoomMeetService : IMeetingService
    {
        private readonly ILogger<ZoomMeetService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ZoomSettings _settings;

        public ZoomMeetService(ILogger<ZoomMeetService> logger, IOptions<ZoomSettings> settings)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _settings = settings.Value;
        }

        public async Task<string> CreateMeetingAsync(DateTime startTime, DateTime endTime, string hostId, string participantId, string title)
        {
            try
            {
                // التنفيذ الفعلي سيحتاج إلى استخدامZoom API
                // هذا تنفيذ بسيط لإظهار الفكرة

                // في بيئة الإنتاج، ستحتاج إلى:
                // 1. استخدام Google OAuth للوصول إلى حساب المضيف
                // 2. إنشاء حدث في Google Calendar مع دعوة المشارك
                // 3. إعداد اجتماع Google Meet وإرجاع الرابط

                // نموذج بسيط لرابط اجتماع
                var meetingId = Guid.NewGuid().ToString("N").Substring(0, 12);
                var meetingLink = $"https://meet.google.com/{meetingId}";

                _logger.LogInformation($"Created meeting: {meetingLink} for host {hostId} and participant {participantId} at {startTime}");

                return meetingLink;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create meeting for host {hostId} and participant {participantId}");
                throw;
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var credentials = $"{_settings.ClientId}:{_settings.ClientSecret}";
                var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://zoom.us/oauth/token?grant_type=account_credentials&account_id={_settings.AccountId}");

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Zoom token request failed: {0}", responseContent);
                    response.EnsureSuccessStatusCode(); // Will throw here with 400
                }


                var tokenResponse = JsonConvert.DeserializeObject<ZoomTokenResponse>(responseContent);
                return tokenResponse?.AccessToken ?? throw new Exception("Access token not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving Zoom access token.");
                throw;
            }
        }

        public async Task<ZoomMeetingResponse> CreateMeetingAsync(DateTime startTime, DateTime endTime, string topic)
        {
            var accessToken = await GetAccessTokenAsync();
            var duration = (int)(endTime - startTime).TotalMinutes;

            var meetingRequest = new ZoomMeetingRequest
            {
                Topic = topic,
                StartTime = startTime.ToUniversalTime(),
                Duration = duration
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{_settings.BaseUrl}/users/me/meetings");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(meetingRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var meetingRequestJson = JsonConvert.SerializeObject(meetingRequest);
            _logger.LogInformation("Meeting request body: {0}", meetingRequestJson);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ZoomMeetingResponse>(content);
        }

    }
}