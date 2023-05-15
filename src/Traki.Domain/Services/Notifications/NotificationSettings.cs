namespace Traki.Domain.Services.Notifications
{
    public class NotificationSettings
    {
        public static readonly string SectionName = "Expo";
        public string NotificationEndpoint { get; set; }
        public string AccessToken { get; set; }
    }
}
