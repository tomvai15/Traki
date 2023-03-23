namespace Traki.Domain.Models
{
    public class ChecklistQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public Evaluation Evaluation { get; set; }

        public int ChecklistId { get; set; }
    }
}
