using Azure.Core;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PuppeteerSharp;
using RazorLight;
using System.Reflection;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Docusign.models;

namespace Traki.Domain.Handlers
{
    public interface IReportHandler
    {
        Task<ProtocolReport> GetProtocolReportInformation(int protocolId);
        Task<byte[]> GetProtocolReport(int protocolId);
        Task<byte[]> GenerateHtmlReport(ProtocolReport protocolReport);
        string GenerateReport();
        Task SignReport(int protocolId, string envelopeId);
        Task ValidateSign(Protocol protocol, DocuSignUserInfo userInfo, string accessToken);
    }

    public class ReportHandler : IReportHandler
    {
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IProtocolRepository _protocolRepository;
        private readonly ISectionHandler _sectionHandler;
        private readonly IStorageService _storageService;
        private readonly IDocuSignService _docuSignService;

        public ReportHandler(ICompaniesRepository companiesRepository,
            IProjectsRepository projectsRepository,
            IProductsRepository productsRepository,
            IProtocolRepository protocolRepository,
            ISectionHandler sectionHandler,
            IStorageService storageService,
            IDocuSignService docuSignService)
        {
            _companiesRepository = companiesRepository;
            _projectsRepository = projectsRepository;
            _productsRepository = productsRepository;
            _protocolRepository = protocolRepository;
            _sectionHandler = sectionHandler;
            _storageService = storageService;
            _docuSignService = docuSignService;
        }

        public async Task<ProtocolReport> GetProtocolReportInformation(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);

            protocol.RequiresToBeNotNullEnity();

            var product = await _productsRepository.GetProduct(protocol.ProductId);
            var project = await _projectsRepository.GetProject(product.ProjectId);
            var company = await _companiesRepository.GetCompany(project.CompanyId);

            var sections = await _sectionHandler.GetSections(protocolId);


            var fullSections = new List<Section>();
            foreach (var section in sections)
            {
                var fullSection = await _sectionHandler.GetSection(section.Id);
                fullSections.Add(fullSection);
            }

            var companyLogo = await _storageService.GetFile("company", company.ImageName);

            var companyLogoBase64 = Convert.ToBase64String(companyLogo.Content.ToArray());

            var protocolReport = new ProtocolReport
            {
                CompanyLogoBase64 = companyLogoBase64,
                Company = company,
                Project = project,
                Product = product,
                Protocol = protocol,
                Sections = fullSections.OrderBy(s => s.Priority).ToList(),
            };

            return protocolReport;
        }

        public async Task<byte[]> GetProtocolReport(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);
            if (protocol.ReportName == null)
            {
                throw new ArgumentException();
            }
            var report = await _storageService.GetFile("company", protocol.ReportName);

            return report.Content.ToArray(); ;
        }

        public async Task<byte[]> GenerateHtmlReport(ProtocolReport protocolReport)
        {
            string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(currentPath + @"\Templates")
                .UseMemoryCachingProvider()
                .Build();

            string result = await engine.CompileRenderAsync("Protocol.cshtml", protocolReport);

            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            string fileName = Guid.NewGuid().ToString() + ".pdf";

            string filePath = currentPath + @"\" + fileName;

            using (var page = await browser.NewPageAsync())
            {
                await page.SetContentAsync(result);
                var r = await page.GetContentAsync();
                await page.PdfAsync(filePath, new PdfOptions { PrintBackground = true });
            }

            // TODO: do in parallel

            var pdfAsBytes = File.ReadAllBytes(filePath);
            MemoryStream stream = new MemoryStream(pdfAsBytes);
            await _storageService.AddFile("company", fileName, "application/pdf", stream);


            var protocol = protocolReport.Protocol;
            protocol.ReportName = fileName;
            await _protocolRepository.UpdateProtocol(protocol);

            return pdfAsBytes;
        }
        public string GenerateReport()
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            // Create an empty page
            PdfPage page = document.AddPage();

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);

            // Draw the text
            gfx.DrawString($"Ataskaita {DateTime.Now.ToString("f")}", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);


            using MemoryStream stream = new MemoryStream();

            document.Save(stream, closeStream: false);

            return Convert.ToBase64String(stream.ToArray());
        }

        public async Task SignReport(int protocolId, string envelopeId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);
            protocol.EnvelopeId = envelopeId;
            await _protocolRepository.UpdateProtocol(protocol);
        }

        public async Task ValidateSign(Protocol protocol, DocuSignUserInfo userInfo, string accessToken)
        {
            string envelopeId = protocol.EnvelopeId;
            string documentId = "3";
            string basePath = userInfo.Accounts.First().BaseUri + "/restapi";
            var result = await _docuSignService.GetDocument(accessToken, basePath, userInfo.Accounts.First().AccountId, envelopeId, documentId);

            // TODO: do in parallel
            await _storageService.AddFile("company", protocol.ReportName, "application/pdf", result);

            protocol.IsSigned = true; ;
            await _protocolRepository.UpdateProtocol(protocol);
        }
    }
}
