using Traki.Domain.Models.Drawing;

namespace Traki.Infrastructure.Entities.Drawing
{
    public class DefectEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public DateTime CreationDate { get; set; }
        public DefectStatus Status { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int DrawingId { get; set; }
        public DrawingEntity Drawing { get; set; }
        public IEnumerable<DefectCommentEntity> DefectComments { get; set; }
        public IEnumerable<DefectNotificationEntity> DefectNotifications { get; set; }
        public IEnumerable<StatusChangeEntity> StatusChanges { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}
