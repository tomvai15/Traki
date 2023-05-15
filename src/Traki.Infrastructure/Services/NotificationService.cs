using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Traki.Domain.Services.Notifications;

namespace Traki.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly NotificationSettings _notificationSettings;

        public NotificationService(HttpClient httpClient, IOptions<NotificationSettings> notificationSettings)
        {
            _httpClient = httpClient;
            _notificationSettings = notificationSettings.Value;
        }

        public async Task SendNotification(string deviceToken, string title, string body)
        {
            var payload = new
            {
                to = deviceToken,
                title = title,
                body = body
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_notificationSettings.NotificationEndpoint, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return;
        }
    }
}
