
namespace Traki.Domain.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
    }
}
