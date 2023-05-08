namespace Traki.Domain.Models
{
    public class ProductRecomendation
    {
        public Product Product { get; set; }
        public int DefectCount { get; set; }
        public int ProtocolsCount { get; set; }
    }
}
