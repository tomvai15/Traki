using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IProductsRepository
    {
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProducts(int projectId);
        Task<Product> CreateProduct(Product product);
    }
}
