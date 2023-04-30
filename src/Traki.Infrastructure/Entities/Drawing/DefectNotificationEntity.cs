namespace Traki.Infrastructure.Entities.Drawing
{
    public class DefectNotificationEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Data { get; set; }
        public string CreationDate { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int DefectId { get; set; }
        public DefectEntity Defect { get; set; }
    }
}
