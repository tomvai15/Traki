namespace Traki.Infrastructure.Entities.Section
{
    public class TableEntity: SectionBase
    {
        public IEnumerable<TableRowEntity> TableRows { get; set; }
    }
}
