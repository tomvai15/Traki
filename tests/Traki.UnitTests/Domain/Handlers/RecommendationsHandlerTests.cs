﻿using FluentAssertions;
using Moq;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;

namespace Traki.UnitTests.Domain.Handlers
{
    public class RecommendationsHandlerTests
    {
        private readonly Mock<IProductsRepository> _productsRepositoryMock;
        private readonly Mock<IDefectsRepository> _defectsRepositoryMock;
        private readonly Mock<IProjectsRepository> _projectsRepository;

        private readonly IRecommendationsHandler _recommendationsHandler;

        public RecommendationsHandlerTests()
        {
            _productsRepositoryMock = new Mock<IProductsRepository>();
            _defectsRepositoryMock = new Mock<IDefectsRepository>();
            _projectsRepository = new Mock<IProjectsRepository>();

            _recommendationsHandler = new RecommendationsHandler(
                _projectsRepository.Object,
                _productsRepositoryMock.Object,
                _defectsRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetRecommendation_WithValidUserId_ReturnsRecommendation()
        {
            // Arrange
            var userId = 1;

            var products = new List<Product>();

            _projectsRepository
                .Setup(x => x.GetProjects())
                .ReturnsAsync(new Project [0]);

            _productsRepositoryMock
                .Setup(x => x.GetProductByQuery(It.IsAny<Func<Product, bool>>()))
                .ReturnsAsync(products);

            var defects = new List<Defect>
            {
                new Defect
                {
                    Id = 1,
                    AuthorId = userId,
                    Drawing = new Drawing
                    {
                        Id = 1,
                        ProductId = 1,
                        Product = new Product { Id = 1, AuthorId = userId, }
                    }
                },
                new Defect
                {
                    Id = 2,
                    AuthorId = userId,
                    Drawing = new Drawing
                    {
                        Id = 2,
                        ProductId = 2,
                        Product = new Product { Id = 2, AuthorId = userId, }
                    }
                }
            };

            _defectsRepositoryMock
                .Setup(x => x.GetDefectsByQuery(It.IsAny<Func<Defect, bool>>()))
                .ReturnsAsync(defects);

            // Act
            var result = await _recommendationsHandler.GetRecommendation(userId);

            // Assert
            result.Products.Should().BeEquivalentTo(products);

        }
    }
}
