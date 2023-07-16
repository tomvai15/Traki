using Traki.Domain.Models.Section;

namespace Traki.Domain.Models
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
