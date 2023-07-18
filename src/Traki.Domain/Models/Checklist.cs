using Traki.Domain.Models.Items;

namespace Traki.Domain.Models
{
    public class Checklist: Section
    {
        public List<Item> Items { get; set; }
    }
}
