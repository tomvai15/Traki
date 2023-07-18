namespace Traki.Api.Contracts.Section.Items
{
    public abstract record ItemBaseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ItemImage { get; set; }
        public int Index { get; set; }
    }
}
