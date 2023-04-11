namespace Traki.Api.Contracts.Section
{
    public class SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public ChecklistDto? Checklist{ get; set; }
        public TableDto? Table { get; set; }
        public int ProtocolId { get; set; }
    }
}
