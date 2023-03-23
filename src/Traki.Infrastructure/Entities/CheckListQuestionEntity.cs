using Traki.Domain.Models;

namespace Traki.Infrastructure.Entities
{
    public class ChecklistQuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; } = string.Empty;
        public Evaluation Evaluation { get; set; } = Evaluation.No;
        public int ChecklistId { get; set; }
        public ChecklistEntity Checklist { get; set; }
    }
}
