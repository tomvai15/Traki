using Traki.Api.Contracts.Section.Items;

namespace Traki.Api.Contracts.Section
{
    public record ChecklistDto: SectionBaseDto
    {
        public ICollection<ItemDto> Items { get; set; }
    }
}
