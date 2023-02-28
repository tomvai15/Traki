using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IProductsHandler
    {
        Task<Product> GetProduct(int productId);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> CreateProduct(Product product);
    }

    public class ProductsHandler : IProductsHandler
    {
        private readonly TrakiDbContext _context;

        public ProductsHandler(TrakiDbContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProduct(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var createdProject = _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return createdProject.Entity;
        }
    }
}
