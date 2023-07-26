using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Domain.Services;

namespace Traki.Domain.Handlers
{
    public interface IProductHandler
    {
        Task AddProtocolToProduct(int productId, int protocolId);
        Task<IEnumerable<Protocol>> GetProtocols(int productId);
    }

    public class ProductHandler : IProductHandler
    {
        private readonly IProtocolService _protocolService;

        public ProductHandler(IProtocolService protocolService)
        {
            _protocolService = protocolService;
        }

        public async Task AddProtocolToProduct(int productId, int protocolId)
        {
        }

        public async Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            return null;
        }
    }
}
