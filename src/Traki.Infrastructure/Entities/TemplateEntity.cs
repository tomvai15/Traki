namespace Traki.Infrastructure.Entities
{
    public class TemplateEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
    }
}
