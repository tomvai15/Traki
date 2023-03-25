namespace Traki.Infrastructure.Entities.Section.Items
{
    public class TextInputEntity
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string ItemId { get; set; }
        public ItemEntity Item { get; set; }
    }
}
