using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;

namespace Traki.Domain.Services
{
    public interface IProtocolService
    {
        Task<Protocol> FindProtocol(int protcolId);
        Task CreateProtocol(Protocol protocol);
        Task UpdateProtocol(Protocol protocol);
        Task<Section> FindSection(int protocolId, int sectionId);
        Task AddSection(Section section);
        Task UpdateSection(Section section);
        Task DeleteSection(Section section);
    }

    public class ProtocolService : IProtocolService
    {

        private readonly IProtocolRepository _protocolRepository;

        public ProtocolService(IProtocolRepository protocolRepository)
        {
            _protocolRepository = protocolRepository;
        }

        public async Task<Protocol> FindProtocol(int protcolId)
        {
            return await _protocolRepository.GetProtocol(protcolId);
        }

        public async Task CreateProtocol(Protocol protocol)
        {
            var newProtocol = protocol.CreateTemplate();
            await _protocolRepository.CreateProtocol(newProtocol);
        }

        public async Task UpdateProtocol(Protocol protocol)
        {
            var protocolToUpdate = await _protocolRepository.GetProtocol(protocol.Id);
            protocolToUpdate.Name = protocol.Name;
            await _protocolRepository.UpdateProtocol(protocol);
        }

        public async Task AddSection(Section section)
        {
            var protocol = await _protocolRepository.GetProtocol(section.ProtocolId);

            protocol.AddSection(section);

            await _protocolRepository.UpdateProtocol(protocol);
        }

        public async Task UpdateSection(Section section)
        {
            var protocol = await _protocolRepository.GetProtocol(section.ProtocolId);

            protocol.UpdateSection(section);

            await _protocolRepository.UpdateProtocol(protocol);
        }

        public async Task<Section> FindSection(int protocolId, int sectionId)
        {
            var protocol = await _protocolRepository.GetProtocol(protocolId);

            return protocol.GetSection(sectionId);
        }

        public async Task DeleteSection(Section section)
        {
            var protocol = await _protocolRepository.GetProtocol(section.ProtocolId);

            protocol.DeleteSection(section.Id);

            await _protocolRepository.UpdateProtocol(protocol);
        }
    }
}
