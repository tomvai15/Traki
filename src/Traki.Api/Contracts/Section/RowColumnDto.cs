namespace Traki.Api.Contracts.Section
{
    public class RowColumnDto
    {
        public int Id { get; set; }
        public int ColumnIndex { get; set; }
        public string Value { get; set; }
        public int TableRowId { get; set; }
    }
}
