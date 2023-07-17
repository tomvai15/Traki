namespace Traki.Infrastructure.Entities.Section
{
    public class SectionBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int ProtocolId { get; set; }
        public ProtocolEntity Protocol { get; set; }
    }
}
