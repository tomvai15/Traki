namespace Traki.Domain.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public IEnumerable<TableRow> TableRows { get; set; }
    }
}
