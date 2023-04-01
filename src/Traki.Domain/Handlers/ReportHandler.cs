using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;

namespace Traki.Domain.Handlers
{
    public interface IReportHandler
    {
        Task<byte[]> GetProtocolReport(int protocolId);
        Task<byte[]> GenerateProtocolReport(int protocolId);
    }

    public class ReportHandler : IReportHandler
    {
        private readonly IProtocolHandler _protocolHandler;
        private readonly IProtocolRepository _protocolRepository;
        private readonly IStorageService _storageService;
        private readonly IReportGenerator _reportGenerator;

        public ReportHandler(IProtocolHandler protocolHandler,
            IProtocolRepository protocolRepository,
            IStorageService storageService,
            IReportGenerator reportGenerator)
        {
            _protocolHandler = protocolHandler;
            _protocolRepository = protocolRepository;
            _storageService = storageService;
            _reportGenerator = reportGenerator;
        }

        public async Task<byte[]> GetProtocolReport(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);
            if (protocol.ReportName == null)
            {
                throw new ArgumentException();
            }
            var reportResult = await _storageService.GetFile("company", protocol.ReportName);

            return reportResult.Content.ToArray();
        }

        public async Task<byte[]> GenerateProtocolReport(int protocolId)
        {
            var protocolInformation = await _protocolHandler.GetInformationForReport(protocolId);

            const string protocolTemplateName = "Protocol.cshtml";
            var htmlReport = await _reportGenerator.GenerateHtmlReport(protocolInformation, protocolTemplateName);

            var pdfStream = await _reportGenerator.GeneratePDFReportFromHtml(htmlReport);

            var protocol = await _protocolRepository.GetProtocol(protocolId);
            string reportName = protocolInformation.Protocol.ReportName ?? $"{Guid.NewGuid().ToString()}.pdf";

            protocol.ReportName = reportName;
            await _protocolRepository.UpdateProtocol(protocol);

            await _storageService.AddFile("company", reportName, "application/pdf", pdfStream);

            return pdfStream.ToArray();
        }
    }
}
