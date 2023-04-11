namespace Traki.Api.Contracts.Section
{
    public class TableRowDto
    {
        public int Id { get; set; }
        public int RowIndex { get; set; }
        public int TableId { get; set; }
        public IEnumerable<RowColumnDto> RowColumns { get; set; }
    }
}
