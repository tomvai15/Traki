using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Entities.Section
{
    public class ChecklistEntity
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public SectionEntity Section { get; set; }
        public ICollection<ItemEntity> Items { get; set; }
    }
}
