using Traki.Api.Models;

namespace Traki.Api.Contracts.ChecklistQuestion
{
    public class ChecklistQuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public Evaluation Evaluation { get; set; }
    }
}
