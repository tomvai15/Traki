namespace Traki.Infrastructure.Entities.Section
{
    public class RowColumnEntity
    {
        public int Id { get; set; }
        public int ColumnIndex { get; set; }
        public string Value { get; set; }
        public int TableRowId { get; set; }
        public TableRowEntity TableRow { get; set; }
    }
}
