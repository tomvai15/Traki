namespace Traki.Api.Contracts.Section
{
    public abstract record SectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public int ProtocolId { get; set; }
        public ChecklistDto? Checklist { get; set; }
        public TableDto? Table { get; set; }
    }
}
