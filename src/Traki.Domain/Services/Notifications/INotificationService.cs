namespace Traki.Domain.Services.Notifications
{
    public interface INotificationService
    {
        public Task SendNotification(string deviceToken, string title, string body);
    }
}
