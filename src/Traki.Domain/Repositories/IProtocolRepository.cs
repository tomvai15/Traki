using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IProtocolRepository
    {
        Task<Protocol> UpdateProtocol(Protocol protocol);
        Task<Protocol> GetProtocol(int protocolId);
        Task DeleteProtocol(int protocolId);
        Task<IEnumerable<Protocol>> GetProtocols(int productId);
        Task<IEnumerable<Protocol>> GetTemplateProtocols();
        Task<Protocol> CreateProtocol(Protocol protocol);
    }
}
