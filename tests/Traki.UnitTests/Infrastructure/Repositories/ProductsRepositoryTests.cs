using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using Product = Traki.Domain.Models.Product;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class ProductsRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public ProductsRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetProducts_ReturnsProducts()
        {
            int projectId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);
            var expectedProducts = await context.Products.Where(x => x.ProjectId == projectId).ToListAsync();

            var products = await repository.GetProducts(projectId);

            expectedProducts.Should().BeEquivalentTo(products, options => options.Excluding(x => x.Author));
        }

        [Fact]
        public async Task GetProduct_ReturnsProduct()
        {
            int productId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);
            var expectedProduct = await context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();

            var product = await repository.GetProduct(productId);

            expectedProduct.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Author));
        }

        [Fact]
        public async Task CreateProduct_CreatesProduct()
        {
            var product = new Product
            {
                Name = Any<string>(),
                Status = Any<string>(),
                CreationDate = Any<string>(),
                ProjectId = 1,
                AuthorId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);

            var cratedProduct = await repository.CreateProduct(product);

            cratedProduct.Should().BeEquivalentTo(product, options => options.Excluding(x => x.Id).Excluding(x => x.CreationDate));
        }

        [Fact]
        public async Task UpdateProducts_UpdatesProducts()
        {
            var product = new Product
            {
                Id = 1,
                Name = Any<string>(),
                Status = Any<string>(),
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);

            var updatedProduct = await repository.UpdateProduct(product);

            updatedProduct.Name.Should().Be(product.Name);
            updatedProduct.Status.Should().Be(product.Status);
        }

        [Fact]
        public async Task DeleteProduct_DeletesProduct()
        {
            // Arrange
            var product = new ProductEntity
            {
                Name = Any<string>(),
                Status = Any<string>(),
                CreationDate = Any<string>(),
                ProjectId = 1,
                AuthorId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);

            context.Products.Add(product);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteProduct(product.Id);

            // Assert
            var foundEntity = await context.Projects.FirstOrDefaultAsync(x => x.Id == product.Id);
            foundEntity.Should().BeNull();
        }

        [Fact]
        public async Task GetProductByQuery_ReturnsSelectedProducts()
        {
            // Arrange
            Func<Product, bool> query = (x) => x.Status == "Active";

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProductsRepository(context, _mapper);

            var expectedProducts = await context.Products.Where(x => x.Status == "Active").ToListAsync();

            // Act
            var products = await repository.GetProductByQuery(query);

            // Assert
            expectedProducts.Should().BeEquivalentTo(products, options => options.Excluding(x => x.Author));
        }
    }
}
