namespace Traki.Api.Contracts.Section
{
    public record TableDto
    {
        public IEnumerable<TableRowDto> TableRows { get; set; }
    }
}
