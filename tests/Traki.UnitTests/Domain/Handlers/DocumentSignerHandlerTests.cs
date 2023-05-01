using DocuSign.eSign.Model;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.DocumentSigning.Models;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Docusign.models;

namespace Traki.UnitTests.Domain.Handlers
{
    public class DocumentSignerHandlerTests
    {
        private readonly Mock<IDocuSignService> _docuSignServiceMock;
        private readonly Mock<IProtocolHandler> _protocolHandlerMock;
        private readonly Mock<IProtocolRepository> _protocolRepositoryMock;
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly Mock<IAccessTokenProvider> _accessTokenProviderMock;
        private readonly Mock<IReportHandler> _reportHandlerMock;

        private readonly DocumentSignerHandler _documentSignerHandler;

        public DocumentSignerHandlerTests()
        {
            _docuSignServiceMock = new Mock<IDocuSignService>();
            _protocolHandlerMock = new Mock<IProtocolHandler>();
            _protocolRepositoryMock = new Mock<IProtocolRepository>();
            _storageServiceMock = new Mock<IStorageService>();
            _accessTokenProviderMock = new Mock<IAccessTokenProvider>();
            _reportHandlerMock = new Mock<IReportHandler>();

            _documentSignerHandler = new DocumentSignerHandler(
                _docuSignServiceMock.Object,
                _protocolHandlerMock.Object,
                _protocolRepositoryMock.Object,
                _storageServiceMock.Object,
                _accessTokenProviderMock.Object,
                _reportHandlerMock.Object
            );
        }

        [Fact]
        public async Task ValidateSign_Should_ThrowArgumentException_When_ProtocolEnvelopeIdIsNull()
        {
            // Arrange
            var protocol = new Protocol { EnvelopeId = null };
            var protocolId = 1;

            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId)).ReturnsAsync(protocol);

            // Act
            Func<Task> act = async () => await _documentSignerHandler.ValidateSign(protocolId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task ValidateSign_Should_CallDependencyMethods_When_ProtocolEnvelopeIdIsNotNull()
        {
            // Arrange
            var protocol = new Protocol { EnvelopeId = "envelopeId" };
            var protocolId = 1;
            var pdfDocument = new MemoryStream( new byte[] { 0x00, 0x01, 0x02 });

            var userInfo = new DocuSignUserInfo();

            _protocolRepositoryMock.Setup(x => x.GetProtocol(protocolId)).ReturnsAsync(protocol);
            _accessTokenProviderMock.Setup(x => x.GetAccessToken()).ReturnsAsync("accessToken");
            _docuSignServiceMock.Setup(x => x.GetUserInformation("accessToken")).ReturnsAsync(userInfo);
            _docuSignServiceMock.Setup(x => x.GetPdfDocument(userInfo, It.IsAny<string>(), "accessToken")).ReturnsAsync(pdfDocument);
            _protocolRepositoryMock.Setup(x => x.UpdateProtocol(protocol));
            _storageServiceMock.Setup(x => x.AddFile("company", protocol.ReportName, "application/pdf", pdfDocument)).Returns(Task.CompletedTask);

            // Act
            await _documentSignerHandler.ValidateSign(protocolId);

            // Assert
            _protocolRepositoryMock.Verify(x => x.GetProtocol(protocolId), Times.Once);
            _accessTokenProviderMock.Verify(x => x.GetAccessToken(), Times.Once);
            _docuSignServiceMock.Verify(x => x.GetUserInformation("accessToken"), Times.Once);
            _docuSignServiceMock.Verify(x => x.GetPdfDocument(It.IsAny<DocuSignUserInfo>(), It.IsAny<string>(), "accessToken"), Times.Once);
            _protocolRepositoryMock.Verify(x => x.UpdateProtocol(protocol), Times.Once);
            _storageServiceMock.Verify(x => x.AddFile("company", protocol.ReportName, "application/pdf", pdfDocument), Times.Once);
        }

        [Fact]
        public async Task SignDocument_Should_CallDependencyMethods_When_ReturnsRedirectUri()
        {
            // Arrange
            var protocol = new Protocol { EnvelopeId = "envelopeId" };
            var protocolId = 1;
            var pdfDocument = new byte[] { 0x00, 0x01, 0x02 };
            string report = Convert.ToBase64String(pdfDocument);
            string state = Any<string>();
            var userInfo = new DocuSignUserInfo();
            string redirectUri = Any<string>();

            _reportHandlerMock.Setup(x => x.GetProtocolReport(protocolId)).ReturnsAsync(pdfDocument);
            _accessTokenProviderMock.Setup(x => x.GetAccessToken()).ReturnsAsync("accessToken");
            _docuSignServiceMock.Setup(x => x.GetUserInformation("accessToken")).ReturnsAsync(userInfo);
            _docuSignServiceMock.Setup(x => x.CreateDocumentSigningRedirectUri(userInfo, "accessToken", report, state))
                .ReturnsAsync(new SignDocumentResult { RedirectUri = redirectUri });

            // Act
            var result = await _documentSignerHandler.SignDocument(protocolId, state);

            // Assert
            result.Should().Be(redirectUri);  
        }
    }
}
