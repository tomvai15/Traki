using Traki.Domain.Models;

namespace Traki.Domain.Services
{
    public interface IProtocolService
    {
        Task<Protocol> FindProtocol(int protcolId);
        Task<Section> FindSection(int protocolId, int sectionId);
        Task AddSection(Section section);
    }

    public class ProtocolService : IProtocolService
    {
        public Task AddSection(Section section)
        {
            throw new NotImplementedException();
        }

        public Task<Protocol> FindProtocol(int protcolId)
        {
            throw new NotImplementedException();
        }

        public Task<Section> FindSection(int protocolId, int sectionId)
        {
            throw new NotImplementedException();
        }
    }
}
