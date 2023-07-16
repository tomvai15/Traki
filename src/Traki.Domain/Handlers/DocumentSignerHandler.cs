using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.Docusign;

namespace Traki.Domain.Handlers
{
    public interface IDocumentSignerHandler
    {
        Task ValidateSign(int protocolId);
        Task<string> SignDocument(int protocolId, string state);
    }
    public class DocumentSignerHandler: IDocumentSignerHandler
    {
        private readonly IDocuSignService _docuSignService;
        private readonly IProtocolHandler _protocolHandler;
        private readonly IProtocolRepository _protocolRepository;
        private readonly IStorageService _storageService;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IReportHandler _reportHandler;

        public DocumentSignerHandler(IDocuSignService docuSignService,
            IProtocolHandler protocolHandler,
            IProtocolRepository protocolRepository,
            IStorageService storageService,
            IAccessTokenProvider accessTokenProvider,
            IReportHandler reportHandler)
        {
            _docuSignService = docuSignService;
            _protocolHandler = protocolHandler;
            _protocolRepository = protocolRepository;
            _storageService = storageService;

            _accessTokenProvider = accessTokenProvider;
            _reportHandler = reportHandler;
        }

        public async Task ValidateSign(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);

            if (protocol.EnvelopeId == null)
            {
                throw new ArgumentException();
            }

            var accessToken = await _accessTokenProvider.GetAccessToken();
            var userInfo = await _docuSignService.GetUserInformation(accessToken);

            string envelopeId = protocol.EnvelopeId;
            var result = await _docuSignService.GetPdfDocument(userInfo, envelopeId, accessToken);

            protocol.IsSigned = true;
            protocol.IsCompleted = true;
            var updateProtocolTask = _protocolRepository.UpdateProtocol(protocol);
            var addFileTask = _storageService.AddFile("company", protocol.ReportName, "application/pdf", result);

            await Task.WhenAll(addFileTask, updateProtocolTask);
        }

        public async Task<string> SignDocument(int protocolId, string state)
        {
            var reportFile = await _reportHandler.GetProtocolReport(protocolId);
            string report = Convert.ToBase64String(reportFile);

            string accessToken = await _accessTokenProvider.GetAccessToken();

            var userInfo = await _docuSignService.GetUserInformation(accessToken);

            var signDocumentResult = await _docuSignService.CreateDocumentSigningRedirectUri(userInfo, accessToken, report, state);

            await _protocolHandler.SignReport(protocolId, signDocumentResult.EnvelopeId);
            return signDocumentResult.RedirectUri;
        }
    }
}
