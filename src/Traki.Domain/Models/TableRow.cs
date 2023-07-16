namespace Traki.Domain.Models
{
    public class TableRow
    {
        public int Id { get; set; }
        public int RowIndex { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; }
        public IEnumerable<RowColumn> RowColumns { get; set; }
    }
}
