namespace Traki.Infrastructure.Entities
{
    public class ProjectEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public string CreationDate { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }
    }
}
