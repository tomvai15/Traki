namespace Traki.Domain.Models.Items
{
    public abstract class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ItemImage { get; set; }
        public int Index { get; set; }
        public int ChecklistId { get; set; }
    }
}
