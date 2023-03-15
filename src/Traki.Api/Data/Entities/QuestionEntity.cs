namespace Traki.Api.Data.Entities
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TemplateId { get; set; }
        public TemplateEntity Template { get; set; }
    }
}
