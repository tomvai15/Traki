namespace Traki.Api.Contracts.Section.Items
{
    public record TextInputDto: ItemBaseDto
    {
        public string Value { get; set; }
    }
}
