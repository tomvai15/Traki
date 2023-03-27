namespace Traki.Infrastructure.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public ICollection<TemplateEntity> Templates { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
    }
}
