namespace Traki.Api.Data.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<TemplateEntity> Templates { get; set; }
    }
}
