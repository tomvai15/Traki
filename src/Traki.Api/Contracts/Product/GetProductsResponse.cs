namespace Traki.Api.Contracts.Product
{
    public class GetProductsResponse
    {
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
