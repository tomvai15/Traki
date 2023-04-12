namespace Traki.Domain.Models.Section
{
    public class RowColumn
    {
        public int Id { get; set; }
        public int ColumnIndex { get; set; }
        public string Value { get; set; }
        public int TableRowId { get; set; }
        public TableRow TableRow { get; set; }
    }
}
