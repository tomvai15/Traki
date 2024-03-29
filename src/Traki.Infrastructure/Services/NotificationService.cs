﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
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

            // expo endpoint is not secured unless push security is enabled
            if (!_notificationSettings.AccessToken.IsNullOrEmpty())
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _notificationSettings.AccessToken);
            }

            var response = await _httpClient.PostAsync(_notificationSettings.NotificationEndpoint, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return;
        }
    }
}
