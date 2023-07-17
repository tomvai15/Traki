namespace Traki.Infrastructure.Entities.Section.Items
{
    public class ItemBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ItemImage { get; set; }
        public int Index { get; set; }
        public int ChecklistId { get; set; }
        public ChecklistEntity Checklist { get; set; }
    }
}
