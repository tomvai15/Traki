using FluentAssertions;
using Moq;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;

namespace Traki.UnitTests.Domain.Handlers
{
    public class ProductHandlerTests
    {
        private readonly Mock<IProtocolRepository> _protocolRepositoryMock;
        private readonly Mock<ISectionHandler> _sectionHandlerMock;
        private readonly IProductHandler _productHandler;

        public ProductHandlerTests()
        {
            _protocolRepositoryMock = new Mock<IProtocolRepository>();
            _sectionHandlerMock = new Mock<ISectionHandler>();

            _productHandler = new ProductHandler(
                _protocolRepositoryMock.Object,
                _sectionHandlerMock.Object
            );
        }

        [Fact]
        public async Task AddProtocolToProduct_Should_Add_New_Protocol_And_Sections()
        {
            // Arrange
            int productId = 1;
            int protocolId = 2;

            var protocol = new Protocol
            {
                Id = protocolId,
                IsTemplate = true
            };

            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId))
                .ReturnsAsync(protocol);

            _protocolRepositoryMock.Setup(x => x.CreateProtocol(It.IsAny<Protocol>()))
                .ReturnsAsync(new Protocol { Id = 3 });

            var sections = new List<Section>
            {
                new Section { Id = 1, ProtocolId = protocolId },
                new Section { Id = 2, ProtocolId = protocolId }
            };

            _sectionHandlerMock.Setup(x => x.GetSections(protocolId))
                .ReturnsAsync(sections);

            _sectionHandlerMock.Setup(x => x.GetSection(It.IsAny<int>()))
                .ReturnsAsync(new Section());

            _sectionHandlerMock.Setup(x => x.AddOrUpdateSection(It.IsAny<int>(), It.IsAny<Section>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productHandler.AddProtocolToProduct(productId, protocolId);

            // Assert
            _protocolRepositoryMock.Verify(x => x.GetProtocol(protocolId), Times.Once);
            _protocolRepositoryMock.Verify(x => x.CreateProtocol(It.IsAny<Protocol>()), Times.Once);

            _sectionHandlerMock.Verify(x => x.GetSections(protocolId), Times.Once);
            _sectionHandlerMock.Verify(x => x.GetSection(It.IsAny<int>()), Times.Exactly(2));
            _sectionHandlerMock.Verify(x => x.AddOrUpdateSection(It.IsAny<int>(), It.IsAny<Section>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetProtocols_Should_Return_Protocols()
        {
            // Arrange
            int productId = 1;

            var protocols = new List<Protocol>
        {
            new Protocol { Id = 1, ProductId = productId },
            new Protocol { Id = 2, ProductId = productId }
        };

            _protocolRepositoryMock.Setup(x => x.GetProtocols(productId))
                .ReturnsAsync(protocols);

            // Act
            var result = await _productHandler.GetProtocols(productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(protocols);
        }
    }

}
