using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Entities
{
    public class ProtocolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsTemplate { get; set; }
        public IEnumerable<SectionEntity> Sections { get; set; }
        public int? ProductId { get; set; }
        public ProductEntity? Product { get; set; }
    }
}
