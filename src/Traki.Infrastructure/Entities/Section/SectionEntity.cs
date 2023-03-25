namespace Traki.Infrastructure.Entities.Section
{
    public class SectionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public ChecklistEntity? Checklist{ get; set; }
        public TableEntity? Table { get; set; }
    }
}
