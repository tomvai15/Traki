namespace Traki.Domain.Models
{
    public class Table: Section
    {
        public IEnumerable<TableRow> TableRows { get; set; }
    }
}
