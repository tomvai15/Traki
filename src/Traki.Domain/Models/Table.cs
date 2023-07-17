namespace Traki.Domain.Models
{
    public class Table: SectionContent
    {
        public int SectionId { get; set; }
        public IEnumerable<TableRow> TableRows { get; set; }
    }
}
