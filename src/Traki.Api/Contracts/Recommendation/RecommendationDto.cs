namespace Traki.Api.Contracts.Recommendation
{
    public class RecommendationDto
    {
        public IEnumerable<DefectRecomendationDto> Defects { get; set; }
        public IEnumerable<ProductRecomendationDto> Products { get; set; }
    }
}
