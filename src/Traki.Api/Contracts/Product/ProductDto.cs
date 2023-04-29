using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Contracts.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public string Status { get; set; }
        public AuthorDto? Author { get; set; }
    }
}
