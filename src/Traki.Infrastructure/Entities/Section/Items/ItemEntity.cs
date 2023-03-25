namespace Traki.Infrastructure.Entities.Section.Items
{
    public class ItemEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public QuestionEntity? Question { get; set; }
        public MultipleChoiceEntity? MultipleChoice { get; set; }
        public TextInputEntity? TextInput { get; set; }
        public ChecklistEntity Checklist { get; set; }
    }
}
