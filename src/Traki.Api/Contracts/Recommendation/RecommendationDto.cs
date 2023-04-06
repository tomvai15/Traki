using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Product;
using Traki.Domain.Models.Drawing;

namespace Traki.Api.Contracts.Recommendation
{
    public class RecommendationDto
    {
        public IEnumerable<DefectDto> Defects { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
