namespace Traki.Api.Contracts.Section
{
    public abstract record SectionBaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int ProtocolId { get; set; }
    }
}
