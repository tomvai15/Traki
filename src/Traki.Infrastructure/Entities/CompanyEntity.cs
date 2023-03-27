namespace Traki.Infrastructure.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ImageName { get; set; }
        public IEnumerable<ProjectEntity> Projects { get; set; }
    }
}
