using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.Recommendation;
using Traki.Api.Controllers;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class RecommendationsControllerTests
    {
        private readonly Mock<IRecommendationsHandler> _recommendationsHandler;
        private readonly Mock<IClaimsProvider> claimsProvider;
        private readonly IMapper _mapper;
        private readonly RecommendationsController _controller;

        public RecommendationsControllerTests()
        {
            _recommendationsHandler = new Mock<IRecommendationsHandler>();
            claimsProvider = new Mock<IClaimsProvider>();
            _mapper = CreateMapper();
            _controller = new RecommendationsController(_recommendationsHandler.Object, claimsProvider.Object, _mapper);
        }

        [Fact]
        public async Task GetRecommendations()
        {
            // Arrange
            var projectId = 1;
            var items = new Recommendation();
            var response = new GetRecommendationResponse
            {
                Recommendation = _mapper.Map<RecommendationDto>(items)
            };

            _recommendationsHandler.Setup(repo => repo.GetRecommendation(It.IsAny<int>()))
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetRecommendations();

            // Assert
            var data = result.ShouldBeOfType<GetRecommendationResponse>();
            response.Should().BeEquivalentTo(data);
        }
    }
}
