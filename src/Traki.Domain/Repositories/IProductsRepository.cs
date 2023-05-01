using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProducts(int projectId);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<IEnumerable<Product>> GetProductByQuery(Func<Product, bool> filter);

        Task DeleteProduct(int productId);
    }
}
