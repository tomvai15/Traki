namespace Traki.Domain.Models
{
    public class Recommendation
    {
        public IEnumerable<DefectRecomendation> Defects { get; set; }
        public IEnumerable<ProductRecomendation> Products { get; set; }
    }
}
