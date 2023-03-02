﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Entities;
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
        private readonly IMapper _mapper;

        public ProductsHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Product> GetProduct(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            return _mapper.Map<Product>(product);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<Product>>(products);
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var productToAdd = _mapper.Map<ProductEntity>(product);
            var createdProduct = _context.Products.Add(productToAdd);
            await _context.SaveChangesAsync();

            return _mapper.Map<Product>(createdProduct.Entity);
        }
    }
}