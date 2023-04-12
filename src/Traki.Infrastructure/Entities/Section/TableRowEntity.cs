namespace Traki.Infrastructure.Entities.Section
{
    public class TableRowEntity
    {
        public int Id { get; set; }
        public int RowIndex { get; set; }
        public int TableId { get; set; }
        public TableEntity Table { get; set; }
        public IEnumerable<RowColumnEntity> RowColumns { get; set; }
    }
}
