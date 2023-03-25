using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Contracts.Section
{
    public class ChecklistDto
    {
        public int Id { get; set; }
        public ICollection<ItemDto> Items { get; set; }
    }
}
