namespace Traki.Domain.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int ProtocolId { get; set; }
        public SectionContent SectionContent { get; set; }
    }
}
