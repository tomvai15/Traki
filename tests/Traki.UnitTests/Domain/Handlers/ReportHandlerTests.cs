using FluentAssertions;
using Moq;
using System.Text;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.FileStorage;

namespace Traki.UnitTests.Domain.Handlers
{
    public class ReportHandlerTests
    {
        private readonly Mock<IProtocolHandler> _protocolHandlerMock;
        private readonly Mock<IProtocolRepository> _protocolRepositoryMock;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly Mock<IReportGenerator> _reportGeneratorMock;
        private readonly ReportHandler _reportHandler;

        public ReportHandlerTests()
        {
            _protocolHandlerMock = new Mock<IProtocolHandler>();
            _protocolRepositoryMock = new Mock<IProtocolRepository>();
            _storageServiceMock = new Mock<IStorageService>();
            _reportGeneratorMock = new Mock<IReportGenerator>();
            _reportHandler = new ReportHandler(_protocolHandlerMock.Object, _protocolRepositoryMock.Object, _storageServiceMock.Object, _reportGeneratorMock.Object);
        }

        [Fact]
        public async Task GetProtocolReport_Should_Return_Report_Content()
        {
            // Arrange
            const int protocolId = 1;
            const string reportName = "test.pdf";
            var protocol = new Protocol { Id = protocolId, ReportName = reportName };
            var fileContent = new byte[] { 1, 2, 3 };
            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId)).ReturnsAsync(protocol);
           _storageServiceMock.Setup(x => x.GetFile("company", reportName)).ReturnsAsync
                ( new GetFileResult { Content = fileContent, ContentType = "test" });

            // Act
            var result = await _reportHandler.GetProtocolReport(protocolId);

            // Assert
            result.Should().BeEquivalentTo(fileContent);
        }

        [Fact]
        public async Task GetProtocolReport_NoReport_ThrowsArgumentException()
        {
            // Arrange
            const int protocolId = 1;
            const string reportName = "test.pdf";
            var protocol = new Protocol { Id = protocolId, ReportName = null };

            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId)).ReturnsAsync(protocol);

            // Act
            Func<Task<byte[]>> getProtocol = async () => await _reportHandler.GetProtocolReport(protocolId);

            // Assert
            getProtocol.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GenerateProtocolReport_ShouldGeneratePDFReport()
        {
            // Arrange
            var fileContent = new byte[] { 1, 2, 3 };
            var protocolId = 1;
            var protocolName = "Test Protocol";
            var useColors = true;
            var sectionsToNotInclude = new List<int> { 2, 4 };
            var protocol = new Protocol { ReportName = "test-report.pdf" };
            var protocolInformation = new ProtocolReport { 
                Protocol = protocol,
                Sections = new List<Section>()
                {
                    new Section() { Id = 1},
                    new Section() { Id = 5},
                }
                };
            var htmlReport = "<html><body><h1>Test Report</h1></body></html>";
            var pdfStream = new MemoryStream(Encoding.UTF8.GetBytes("Test PDF Report"));
            var expectedReportName = "test-report.pdf";

            _protocolHandlerMock.Setup(x => x.GetInformationForReport(protocolId))
                               .ReturnsAsync(protocolInformation);

            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId))
                                  .ReturnsAsync(protocol);

            _protocolRepositoryMock.Setup(x => x.UpdateProtocol(protocol));

            _storageServiceMock.Setup(x => x.AddFile("company", expectedReportName, "application/pdf", pdfStream))
                              .Returns(Task.CompletedTask);
            _storageServiceMock.Setup(x => x.GetFile("company", protocol.ReportName))
                              .ReturnsAsync(new GetFileResult { Content = fileContent, ContentType = "test" });

            _reportGeneratorMock.Setup(x => x.GenerateHtmlReport(protocolInformation, "Protocol.cshtml"))
                                .ReturnsAsync(htmlReport);
            _reportGeneratorMock.Setup(x => x.GeneratePDFReportFromHtml(htmlReport, useColors))
                                .ReturnsAsync(pdfStream);

            // Act
            var actualResult = await _reportHandler.GenerateProtocolReport(protocolId, protocolName, useColors, sectionsToNotInclude);

            // Assert
            actualResult.Should().NotBeNull();
            actualResult.Should().BeEquivalentTo(pdfStream.ToArray());
            protocol.ReportName.Should().NotBeNull();
            protocol.ReportName.Should().Be(expectedReportName);
        }
    }
}
