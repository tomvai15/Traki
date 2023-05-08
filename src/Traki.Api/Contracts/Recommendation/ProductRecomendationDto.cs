using Traki.Api.Contracts.Product;

namespace Traki.Api.Contracts.Recommendation
{
    public class ProductRecomendationDto
    {
        public ProductDto Product { get; set; }
        public int DefectCount { get; set; }
        public int ProtocolsCount { get; set; }
    }
}
