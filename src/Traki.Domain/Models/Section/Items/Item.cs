namespace Traki.Domain.Models.Section.Items
{
    public class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ItemImage { get; set; }
        public int Priority { get; set; }
        public Question? Question { get; set; }
        public MultipleChoice? MultipleChoice { get; set; }
        public TextInput? TextInput { get; set; }
        public int ChecklistId { get; set; }
    }
}
