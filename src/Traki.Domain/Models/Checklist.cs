using Traki.Domain.Models.Items;

namespace Traki.Domain.Models
{
    public class Checklist: ISectionContent
    {
        public List<Item> Items { get; set; }
    }
}
