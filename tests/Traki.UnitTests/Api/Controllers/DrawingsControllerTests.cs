using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Controllers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class DrawingsControllerTests
    {
        private readonly Mock<IDrawingsRepository> drawingsRepository;
        private readonly IMapper _mapper;
        private readonly DrawingsController _controller;

        public DrawingsControllerTests()
        {
            drawingsRepository = new Mock<IDrawingsRepository>();
            _mapper = CreateMapper();
            _controller = new DrawingsController(drawingsRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetDrawings()
        {
            // Arrange
            var productId = 1;
            var drawings = new List<Drawing>();
            var response = new GetDrawingsResponse
            {
                Drawings = _mapper.Map<IEnumerable<DrawingDto>>(drawings)
            };

            drawingsRepository.Setup(repo => repo.GetDrawings(productId))
                .ReturnsAsync(drawings);

            // Act
            var result = await _controller.GetDrawings(productId);

            // Assert
            var data = result.ShouldBeOfType<GetDrawingsResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task GetDrawing()
        {
            // Arrange
            var productId = 1;
            var drawingId = 1;
            var drawing = new Drawing();
            var response = new GetDrawingResponse
            {
                Drawing = _mapper.Map<DrawingDto>(drawing)
            };

            drawingsRepository.Setup(repo => repo.GetDrawing(drawingId))
                .ReturnsAsync(drawing);

            // Act
            var result = await _controller.GetDrawing(productId, drawingId);

            // Assert
            var data = result.ShouldBeOfType<GetDrawingResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task DeleteDrawing()
        {
            // Arrange
            var drawingId = 1;
            var drawing = new Drawing();
            var response = new GetDrawingResponse
            {
                Drawing = _mapper.Map<DrawingDto>(drawing)
            };

            drawingsRepository.Setup(repo => repo.DeleteDrawing(drawingId));

            // Act
            var result = await _controller.DeleteDrawing(drawingId);

            // Assert

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task CreateDrawing()
        {
            // Arrange
            var productId = 1;
            var request = new CreateDrawingRequest
            {
                Drawing = new DrawingDto { Id = productId }
            };

            drawingsRepository.Setup(repo => repo.CreateDrawing(It.IsAny<Drawing>()));

            // Act
            var result = await _controller.CreateDrawing(productId, request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
