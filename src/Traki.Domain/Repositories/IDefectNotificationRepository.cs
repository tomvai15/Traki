using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IDefectNotificationRepository
    {
        Task<IEnumerable<DefectNotification>> GetUserDefectNotifications(int userId);
        Task<DefectNotification> CreateDefectNotification(DefectNotification defectNotification);
        Task DeleteDefectNotification(int userId, int defectId);
    }
}
