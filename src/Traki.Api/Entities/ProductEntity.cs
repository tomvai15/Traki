using Traki.Api.Models;

namespace Traki.Api.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public ProjectEntity Project { get; set; }
    }
}
