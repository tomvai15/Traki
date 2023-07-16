using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Traki.Domain.Extensions;

namespace Traki.Infrastructure.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ProductsRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            productEntity.RequiresToBeNotNullEnity();
            productEntity.Name = product.Name;
            productEntity.Status = product.Status;

            await _context.SaveChangesAsync();
            return _mapper.Map<Product>(productEntity);
        }

        public async Task DeleteProduct(int productId)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            productEntity.RequiresToBeNotNullEnity();

            var products = await _context.Products.Where(p => p.Id == productId).Include(x => x.Protocols).ToListAsync();

            var protocols = products.SelectMany(x => x.Protocols).ToList();
            _context.RemoveRange(protocols);

            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int productId)
        {
            var product = await _context.Products.Include(x => x.Author)
                .FirstOrDefaultAsync(p => p.Id == productId);

            return _mapper.Map<Product>(product);
        }

        public async Task<IEnumerable<Product>> GetProducts(int projectId)
        {
            var project = await _context.Projects
               .Where(x => x.Id == projectId)
               .Include(x => x.Products)
               .ThenInclude(x=> x.Author)
               .FirstOrDefaultAsync();

            project.RequiresToBeNotNullEnity();

            var products = project.Products.ToList();
            return _mapper.Map<IEnumerable<Product>>(products);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var productToAdd = _mapper.Map<ProductEntity>(product);

            productToAdd.CreationDate = DateTime.Now.ToString("s");

            var createdProduct = _context.Products.Add(productToAdd);
            await _context.SaveChangesAsync();

            return _mapper.Map<Product>(createdProduct.Entity);
        }

        public async Task<IEnumerable<Product>> GetProductByQuery(Func<Product, bool> filter)
        {
            Func<ProductEntity, bool> func = (x) => {
                var p = _mapper.Map<Product>(x);
                return filter(p);
            }; 
            var products = _context.Products.Include(x=> x.Drawings).ThenInclude(x=>x.Defects).Include(x=> x.Protocols)
                .Where(func).ToList();

            return _mapper.Map<IEnumerable<Product>>(products);
        }
    }
}
