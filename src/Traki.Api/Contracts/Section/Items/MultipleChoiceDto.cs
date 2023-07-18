namespace Traki.Api.Contracts.Section.Items
{
    public record MultipleChoiceDto: ItemBaseDto
    {
        public List<OptionDto> Options { get; set; }
    }
}
