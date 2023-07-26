using Traki.Domain.Models.Section.Items;

namespace Traki.Domain.Models.Section
{
    public class Checklist
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public List<Item> Items { get; set; }
    }
}
