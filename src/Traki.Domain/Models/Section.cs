namespace Traki.Domain.Models
{
    public abstract class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int ProtocolId { get; set; }
        public ISectionContent SectionContent { get; set; }
    }
}
