namespace Traki.Domain.Models.Drawing
{
    public class DefectNotification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Data { get; set; }
        public int UserId { get; set; }
        public string CreationDate { get; set; }
        public User User { get; set; }
        public int DefectId { get; set; }
        public Defect Defect { get; set; }
    }
}
