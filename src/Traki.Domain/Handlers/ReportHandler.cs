using Azure.Core;
using Microsoft.IdentityModel.Tokens;
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

            var product = await _productsRepository.GetProduct(protocol.ProductId.Value);
            var project = await _projectsRepository.GetProject(product.ProjectId);
            var company = await _companiesRepository.GetCompany(project.CompanyId);

            var sections = await _sectionHandler.GetSections(protocolId);


            var fullSections = new List<Section>();
            foreach (var section in sections)
            {
                var fullSection = await _sectionHandler.GetSection(section.Id);
                fullSections.Add(fullSection);
            }

            fullSections = fullSections.OrderBy(s => s.Priority).ToList();

            var itemsWithImages = fullSections.Where(x => x.Checklist != null)
                .Select(x => x.Checklist)
                .SelectMany(x => x.Items)
                .Where(x => !x.ItemImage.IsNullOrEmpty())
                .ToList();


            List<ItemImage> itemImages = new List<ItemImage>();
            int num = 1;
            foreach (var item in itemsWithImages)
            {
                var image = await _storageService.GetFile("item", item.ItemImage);
                var imageBase64 = Convert.ToBase64String(image.Content.ToArray());
                itemImages.Add(new ItemImage
                {
                    ImageBase64 = imageBase64,
                    ItemId = item.Id,
                    AttachmentName = $"Attachment {num}",
                });
                num++;
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
                Sections = fullSections,
                ItemImages = itemImages,
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
                await page.PdfAsync(filePath, new PdfOptions { PrintBackground = true });
            }

            var pdfAsBytes = File.ReadAllBytes(filePath);
            MemoryStream stream = new MemoryStream(pdfAsBytes);
            var addFileTask = _storageService.AddFile("company", fileName, "application/pdf", stream);

            var protocol = protocolReport.Protocol;
            protocol.ReportName = fileName;
            var updateProtocolTask = _protocolRepository.UpdateProtocol(protocol);

            await Task.WhenAll(addFileTask, updateProtocolTask);

            return pdfAsBytes;
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

            var addFileTask = _storageService.AddFile("company", protocol.ReportName, "application/pdf", result);

            protocol.IsSigned = true; ;
            var updateProtocolTask =  _protocolRepository.UpdateProtocol(protocol);

            await Task.WhenAll(addFileTask, updateProtocolTask);
        }
    }
}
