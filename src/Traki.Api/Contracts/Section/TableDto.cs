namespace Traki.Api.Contracts.Section
{
    public class TableDto
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public IEnumerable<TableRowDto> TableRows { get; set; }
    }
}
