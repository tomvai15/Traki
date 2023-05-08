using Moq;
using Traki.Domain.Handlers;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.FileStorage;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Domain.Handlers
{
    public class ProtocolHandlerTests
    {
        private readonly Mock<IClaimsProvider> _claimsProvider = new Mock<IClaimsProvider>();
        private readonly Mock<ICompaniesRepository> _companiesRepository = new Mock<ICompaniesRepository>();
        private readonly Mock<IProjectsRepository> _projectsRepository = new Mock<IProjectsRepository>();
        private readonly Mock<IProductsRepository> _productsRepository = new Mock<IProductsRepository>();
        private readonly Mock<IProtocolRepository> _protocolRepository = new Mock<IProtocolRepository>();
        private readonly Mock<ISectionHandler> _sectionHandler = new Mock<ISectionHandler>();
        private readonly Mock<IStorageService> _storageService = new Mock<IStorageService>();

        private readonly ProtocolHandler _protocolHandler;
        public ProtocolHandlerTests() 
        {
            _protocolHandler = new ProtocolHandler(_claimsProvider.Object, _companiesRepository.Object, _projectsRepository.Object,
                    _productsRepository.Object, _protocolRepository.Object, _sectionHandler.Object, _storageService.Object);
        }

        [Fact]
        public async Task GetInformationForReport()
        {
            int protocolId = 1;

            var getFileResult = Any<GetFileResult>();

            _protocolRepository.Setup(x => x.GetProtocol(It.IsAny<int>())).ReturnsAsync(MockData.Protocols.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());
            _projectsRepository.Setup(x => x.GetProject(It.IsAny<int>())).ReturnsAsync(MockData.Projects.First());
            _companiesRepository.Setup(x => x.GetCompany(It.IsAny<int>())).ReturnsAsync(MockData.Companies.First());

            _sectionHandler.Setup(x => x.GetSections(It.IsAny<int>())).ReturnsAsync(MockData.Sections);
            _sectionHandler.Setup(x => x.GetSection(It.IsAny<int>())).ReturnsAsync(MockData.Sections.First());

            _storageService.Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(getFileResult);

            await _protocolHandler.GetInformationForReport(protocolId);

            Assert.True(true);
        }

        [Fact]
        public async Task SignReport()
        {
            int protocolId = 1;
            string envelopeId = Any<string>();

            _protocolRepository.Setup(x => x.GetProtocol(It.IsAny<int>())).ReturnsAsync(MockData.Protocols.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());
            _projectsRepository.Setup(x => x.GetProject(It.IsAny<int>())).ReturnsAsync(MockData.Projects.First());
            _companiesRepository.Setup(x => x.GetCompany(It.IsAny<int>())).ReturnsAsync(MockData.Companies.First());

            _sectionHandler.Setup(x => x.GetSections(It.IsAny<int>())).ReturnsAsync(MockData.Sections);
            _sectionHandler.Setup(x => x.GetSection(It.IsAny<int>())).ReturnsAsync(MockData.Sections.First());


            await _protocolHandler.SignReport(protocolId, envelopeId);

            Assert.True(true);
        }
    }
}
