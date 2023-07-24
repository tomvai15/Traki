using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Contracts.Section
{
    public record ChecklistDto
    {
        public List<ItemDto> Items { get; set; }
    }
}
