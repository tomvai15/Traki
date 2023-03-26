using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;

namespace Traki.Infrastructure.Repositories
{
    public class ProtocolRepository : IProtocolRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ProtocolRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Protocol> CreateProtocol()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProtocol(Protocol protocol)
        {
            var protocolEntity = await _context.Protocols.FirstOrDefaultAsync(p => p.Id == protocol.Id);

            protocolEntity.Name = protocol.Name;
            await _context.SaveChangesAsync();
        }

        public async Task<Protocol> GetProtocol(int protocolId)
        {
            var protocol = await _context.Protocols.FirstOrDefaultAsync(p => p.Id == protocolId);

            protocol.RequiresToBeNotNullEnity();

            return _mapper.Map<Protocol>(protocol);
        }

        public async Task<IEnumerable<Protocol>> GetTemplateProtocols()
        {
            var protocols = await _context.Protocols.Where(p =>p.IsTemplate == true).ToListAsync();

            return _mapper.Map<IEnumerable<Protocol>>(protocols);
        }

        public Task<Protocol> UpdateProtocol()
        {
            throw new NotImplementedException();
        }
    }
}
