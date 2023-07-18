namespace Traki.Api.Contracts.Section
{
    public record TableDto: SectionBaseDto
    {
        public IEnumerable<TableRowDto> TableRows { get; set; }
    }
}
