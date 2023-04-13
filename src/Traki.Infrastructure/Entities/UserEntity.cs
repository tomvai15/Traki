using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string? EncryptedRefreshToken { get; set; }
        public IEnumerable<ProjectEntity> Projects { get; set; }
        public IEnumerable<ProductEntity> Products { get; set; }
        public IEnumerable<DefectEntity> Defects { get; set; }
        public IEnumerable<DefectCommentEntity> DefectComments { get; set; }
        public IEnumerable<StatusChangeEntity> StatusChanges { get; set; }
    }
}
