namespace Traki.Api.Contracts.Section.Items
{
    public record MultipleChoiceDto: ItemDto
    {
        public List<OptionDto> Options { get; set; }
    }
}
