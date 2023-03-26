namespace Traki.Domain.Models.Section
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public Checklist? Checklist{ get; set; }
        public Table? Table { get; set; }

        public int ProtocolId { get; set; }
        public Protocol Protocol { get; set; }
    }
}
