using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Api.Contracts.Product;
using Traki.Api.Controllers;
using Traki.Api.Mapping;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.UnitTests.Api.Controllers
{
    
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsRepository> _productsRepositoryMock;
        private readonly Mock<IProductHandler> _productHandlerMock;
        private readonly IMapper _mapper;
        private readonly ProductsController _productsController;

        public ProductsControllerTests()
        {
            IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToDomainModelMappingProfile());
                cfg.AddProfile(new DomainToContractMappingProfile());
            });

            _mapper = new Mapper(configuration);
            _productsRepositoryMock = new Mock<IProductsRepository>();
            _productHandlerMock = new Mock<IProductHandler>();
            _productsController = new ProductsController(_productsRepositoryMock.Object, _productHandlerMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductIsNull()
        {
            // Arrange
            int projectId = 1;
            int productId = 2;
            _productsRepositoryMock.Setup(x => x.GetProduct(productId)).ReturnsAsync(null as Product);

            // Act
            var result = await _productsController.GetProduct(projectId, productId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact(Skip = "sdasad")]
        public async Task GetProduct_ReturnsMappedProduct_WhenProductIsNotNull()
        {
            // Arrange
            int projectId = 1;
            int productId = 2;
            var product = new Product { Id = productId };
            var getProductResponse = new GetProductResponse { Product = _mapper.Map<ProductDto>(product) };
            _productsRepositoryMock.Setup(x => x.GetProduct(productId)).ReturnsAsync(product);

            // Act
            var result = await _productsController.GetProduct(projectId, productId);

            // Assert
            result.Value.Should().BeEquivalentTo(getProductResponse);
        }

        [Fact(Skip = "sdasad")]
        public async Task UpdateProduct_ReturnsOkResult_WhenUpdateProductRequestIsValid()
        {
            // Arrange
            int projectId = 1;
            int productId = 2;
            var updateProductRequest = new UpdateProductRequest { Product = new ProductDto() };

            // Act
            var result = await _productsController.UpdateProduct(projectId, productId, updateProductRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetProducts_ReturnsMappedProducts()
        {
            // Arrange
            int projectId = 1;
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            var getProductsResponse = new GetProductsResponse { Products = new List<ProductDto> { new ProductDto { Id = 1 }, new ProductDto { Id = 2 } } };
            _productsRepositoryMock.Setup(x => x.GetProducts(projectId)).ReturnsAsync(products);

            // Act
            var result = await _productsController.GetProducts(projectId);

            // Assert
            result.Value.Should().BeEquivalentTo(getProductsResponse);
        }
    }
}
