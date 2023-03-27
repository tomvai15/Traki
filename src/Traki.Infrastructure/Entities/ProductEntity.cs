namespace Traki.Infrastructure.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
        public ICollection<OldChecklistEntity> CheckLists { get; set; }
        public ICollection<ProtocolEntity> Protocols { get; set; }
    }
}
