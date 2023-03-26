using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface IProductHandler
    {
        Task AddProtocolToProduct(int productId, int protocolId);

        Task<IEnumerable<Protocol>> GetProtocols(int productId);
    }

    public class ProductHandler : IProductHandler
    {
        public IProtocolRepository _protocolRepository { get; set; }
        public ISectionHandler _sectionHandler { get; set; }

        public ProductHandler(IProtocolRepository protocolRepository, ISectionHandler sectionHandler)
        {
            _protocolRepository = protocolRepository;
            _sectionHandler = sectionHandler;
        }

        public async Task AddProtocolToProduct(int productId, int protocolId)
        {
            var newProtocol = await _protocolRepository.GetProtocol(protocolId);

            newProtocol.IsTemplate = false;
            newProtocol.ProductId = productId;
            newProtocol.Id = 0;

            newProtocol = await _protocolRepository.CreateProtocol(newProtocol);

            var sections = await _sectionHandler.GetSections(protocolId);

            foreach (var section in sections)
            {
                section.Id = 0;
                section.ProtocolId = newProtocol.Id;
                await _sectionHandler.AddOrUpdateSection(newProtocol.Id, section);
            }
        }

        public async Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            return await _protocolRepository.GetProtocols(productId);
        }
    }
}
