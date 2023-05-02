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
        public IEnumerable<Protocol> Protocols { get; set; }
        public IEnumerable<Traki.Domain.Models.Drawing.Drawing> Drawings { get; set; }
    }
}
