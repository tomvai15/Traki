namespace Traki.Domain.Models
{
    public class Table: ISectionContent
    {
        public IEnumerable<TableRow> TableRows { get; set; }
    }
}
