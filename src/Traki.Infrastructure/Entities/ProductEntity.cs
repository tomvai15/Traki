using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
        public ICollection<OldChecklistEntity> CheckLists { get; set; }
        public ICollection<ProtocolEntity> Protocols { get; set; }
        public ICollection<DrawingEntity> Drawings { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}
