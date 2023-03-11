namespace Traki.Api.Entities
{
    public class ChecklistQuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ChecklistId { get; set; }
        public ChecklistEntity Checklist { get; set; }
    }
}
