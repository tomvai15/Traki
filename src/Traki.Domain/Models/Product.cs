namespace Traki.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string CreationDate { get; set; }
        public int ProjectId { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
    }
}
