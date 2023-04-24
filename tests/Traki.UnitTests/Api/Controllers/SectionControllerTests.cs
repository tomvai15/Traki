using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Section;
using Traki.Api.Controllers;
using Traki.Api.Mapping;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Section;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class SectionControllerTests
    {
        private readonly Mock<ISectionHandler> _mockSectionHandler;
        private readonly IMapper _mapper;
        private readonly SectionController _sectionController;

        public SectionControllerTests()
        {
            _mockSectionHandler = new Mock<ISectionHandler>();
            IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToDomainModelMappingProfile());
                cfg.AddProfile(new DomainToContractMappingProfile());
            });

            _mapper = new Mapper(configuration);
            _sectionController = new SectionController(_mockSectionHandler.Object, _mapper);
        }

        [Fact]
        public async Task UpdateSection_ReturnsOkResult()
        {
            // Arrange
            var protocolId = 1;
            var updateSectionRequest = new UpdateSectionRequest { Section = new SectionDto() };

            _mockSectionHandler.Setup(x => x.AddOrUpdateSection(It.IsAny<int>(), It.IsAny<Section>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sectionController.UpdateSection(protocolId, updateSectionRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateSectionAnswers_ReturnsOkResult()
        {
            // Arrange
            var protocolId = 1;
            var updateSectionAnswersRequest = new UpdateSectionAnswersRequest { Section = new SectionDto() };

            _mockSectionHandler.Setup(x => x.UpdateSectionAnswers(It.IsAny<int>(), It.IsAny<Section>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sectionController.UpdateSectionAnswers(protocolId, updateSectionAnswersRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task CreateSection_ReturnsOkResult()
        {
            // Arrange
            var protocolId = 1;
            var updateSectionRequest = new UpdateSectionRequest { Section = new SectionDto() };

            _mockSectionHandler.Setup(x => x.AddOrUpdateSection(It.IsAny<int>(), It.IsAny<Section>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sectionController.CreateSection(protocolId, updateSectionRequest);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetSection_ReturnsOkResultWithGetSectionRequest()
        {
            // Arrange
            var sectionId = 1;
            var section = new Section();
            var sectionDto = new SectionDto();
            var getSectionRequest = new GetSectionRequest { Section = sectionDto };

            _mockSectionHandler.Setup(x => x.GetSection(sectionId))
                .ReturnsAsync(section);

            // Act
            var result = await _sectionController.GetSection(sectionId);

            // Assert

            var data = result.ShouldBeOfType<GetSectionRequest>();
            data.Should().BeEquivalentTo(getSectionRequest);
        }

        [Fact]
        public async Task DeleteSection_ReturnsOkResult()
        {
            // Arrange
            var sectionId = 1;

            _mockSectionHandler.Setup(x => x.DeleteSection(sectionId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sectionController.DeleteSection(sectionId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
