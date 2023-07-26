using Microsoft.IdentityModel.Tokens;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Models.Section.Items;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.BlobStorage;

namespace Traki.Domain.Handlers
{
    public interface IProtocolHandler
    {
        Task<ProtocolReport> GetInformationForReport(int protocolId);
        Task SignReport(int protocolId, string envelopeId);
    }

    public class ProtocolHandler: IProtocolHandler
    {
        private readonly IClaimsProvider _claimsProvider;
        private readonly ICompaniesRepository _companiesRepository;
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IProtocolRepository _protocolRepository;
        private readonly IStorageService _storageService;

        public ProtocolHandler(IClaimsProvider claimsProvider, ICompaniesRepository companiesRepository, IProjectsRepository projectsRepository, IProductsRepository productsRepository, IProtocolRepository protocolRepository, IStorageService storageService)
        {
            _claimsProvider = claimsProvider;
            _companiesRepository = companiesRepository;
            _projectsRepository = projectsRepository;
            _productsRepository = productsRepository;
            _protocolRepository = protocolRepository;
            _storageService = storageService;
        }

        public async Task SignReport(int protocolId, string envelopeId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);
            _claimsProvider.TryGetUserId(out int userId);
            protocol.EnvelopeId = envelopeId;
            protocol.SignerId = userId;
            await _protocolRepository.UpdateProtocol(protocol);
        }

        public async Task<ProtocolReport> GetInformationForReport(int protocolId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);
            protocol.RequiresToBeNotNullEnity();

            var product = await _productsRepository.GetProduct(protocol.ProductId.Value);
            var project = await _projectsRepository.GetProject(product.ProjectId);
            var company = await _companiesRepository.GetCompany(project.CompanyId);
            var sections = protocol.Sections;

            var fullSections = await GetSections(sections);

            var itemImages = await GetItemImages(fullSections);

            var companyLogo = await _storageService.GetFile("company", company.ImageName);
            var companyLogoBase64 = Convert.ToBase64String(companyLogo.Content);

            var protocolReport = new ProtocolReport
            {
                ProtocolName = protocol.Name,
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

        private async Task<List<Section>> GetSections(IEnumerable<Section> sections)
        {
            List<Section> fullSections = new List<Section>();

            foreach (var section in sections)
            {
                //var fullSection = await _sectionHandler.GetSection(section.Id);
             //   fullSections.Add(fullSection);
            }

            return fullSections.OrderBy(s => s.Priority).ToList();
        }

        private async Task<List<ItemImage>> GetItemImages(List<Section> sections)
        {
            var itemsWithImages = sections.Where(x => x.Checklist != null)
                .Select(x => x.Checklist)
                .SelectMany(x => x.Items)
                .Where(x => !x.ItemImage.IsNullOrEmpty())
                .ToList();

            var getItemImagesTasks = itemsWithImages.Select(x => GetItemImage(x));

            var itemImages = await Task.WhenAll(getItemImagesTasks);
            int num = 1;

            foreach (var itemImage in itemImages)
            {
                itemImage.AttachmentName = $"Attachment {num}";
                num++;
            }

            return itemImages.ToList();
        }

        private async Task<ItemImage> GetItemImage(Item item)
        {
            var image = await _storageService.GetFile("item", item.ItemImage);
            var imageBase64 = Convert.ToBase64String(image.Content.ToArray());
            return new ItemImage
            {
                ImageBase64 = imageBase64,
                ItemId = item.Id
            };
        }
    }
}
