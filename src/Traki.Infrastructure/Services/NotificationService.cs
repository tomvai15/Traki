using Expo.Server.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Traki.Domain.Services;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Notifications;

namespace Traki.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
       // private readonly PushApiClient _pushApiClient = new PushApiClient();

        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendNotification()
        {
            var payload = new
            {
                to = "ExponentPushToken[X_Ov7mI0JUS43mxirmksWW]",
                title = "c#",
                body = "zjbs"
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://exp.host/--/api/v2/push/send", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            return;
        }
        /*
        public async Task SendNotification()
        {
            var request = new Expo.Server.Models.PushTicketRequest
            {
                PushTo = new List<string>() { "ExponentPushToken[X_Ov7mI0JUS43mxirmksWW]" },
                PushData = "test",
                PushBody = "byb",
                PushTitle = "kiaus",
            };
            var a = await _pushApiClient.PushSendAsync(request);

            return;
        }*/
    }
}
