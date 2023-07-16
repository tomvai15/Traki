using Traki.Domain.Models.Items;

namespace Traki.Domain.Models
{
    public class Checklist
    {
        public int Id { get; set; }
        public ICollection<Item> Items { get; set; }
        public int SectionId { get; set; }
    }
}
