namespace Traki.Api.Contracts.Drawing.Defect
{
    public class DefectNotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Data { get; set; }
        public int UserId { get; set; }
        public int DefectId { get; set; }
    }
}
