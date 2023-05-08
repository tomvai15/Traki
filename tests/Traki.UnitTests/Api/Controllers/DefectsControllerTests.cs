using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Controllers;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class DefectsControllerTests
    {
        private readonly Mock<IDefectHandler> _defectHandler = new Mock<IDefectHandler>();
        private readonly Mock<IDefectsRepository> _defectsRepository = new Mock<IDefectsRepository>();
        private readonly Mock<IClaimsProvider> _claimsProvider = new Mock<IClaimsProvider>();
        private readonly IMapper _mapper;
        private readonly DefectsController _controller;

        public DefectsControllerTests()
        {
            _mapper = CreateMapper();
            _controller = new DefectsController(_defectHandler.Object, _defectsRepository.Object, _claimsProvider.Object, _mapper);
        }


        [Fact]
        public async Task CreateDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var request = new CreateDefectRequest
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefect(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Defect>())).ReturnsAsync(items);

            // Act
            var result = await _controller.CreateDefect(drawingId, request);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task UpdateDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var request = new CreateDefectRequest
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefectStatusChange(It.IsAny<int>(), It.IsAny<Defect>())).ReturnsAsync(items);

            // Act
            var result = await _controller.UpdateDefect(drawingId, defectId, request);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }


        [Fact]
        public async Task GetDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(items);

            // Act
            var result = await _controller.GetDefect(drawingId, defectId);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task AddDefectComment()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = new DefectComment();

            var request = new CreateDefectCommentRequest
            {
                DefectComment = _mapper.Map<DefectCommentDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefectComment(It.IsAny<int>(), It.IsAny<DefectComment>()));

            // Act
            var result = await _controller.AddDefectComment(drawingId, defectId, request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
