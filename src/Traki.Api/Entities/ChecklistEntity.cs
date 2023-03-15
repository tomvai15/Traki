namespace Traki.Api.Entities
{
    public class ChecklistEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public ICollection<ChecklistQuestionEntity> ChecklistQuestions { get; set; }
    }
}
