using Traki.Domain.Models.Items;

namespace Traki.Domain.Models
{
    public class Checklist: SectionContent
    {
        public ICollection<Item> Items { get; set; }
        public int SectionId { get; set; }
    }
}
