namespace Traki.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
    }
}
