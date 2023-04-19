namespace Traki.Api.Contracts.Drawing.Defect
{
    public class GetDefectNotificationsResponse
    {
        public IEnumerable<DefectNotificationDto> DefectNotifications { get; set; }
    }
}
