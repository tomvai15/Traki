using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;

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

        public async Task<Protocol> CreateProtocol(Protocol protocol)
        {
            var protocolEntity = _mapper.Map<ProtocolEntity>(protocol);
            protocolEntity = (_context.Protocols.Add(protocolEntity)).Entity;
            await _context.SaveChangesAsync();

            return _mapper.Map<Protocol>(protocolEntity);
        }

        public async Task UpdateProtocol(Protocol protocol)
        {
            var protocolEntity = await _context.Protocols.FirstOrDefaultAsync(p => p.Id == protocol.Id);

            protocolEntity.Name = protocol.Name;
            protocolEntity.ReportName = protocol.ReportName;
            protocolEntity.EnvelopeId = protocol.EnvelopeId;
            await _context.SaveChangesAsync();
        }

        public async Task<Protocol> GetProtocol(int protocolId)
        {
            var protocol = await _context.Protocols.FirstOrDefaultAsync(p => p.Id == protocolId);

            protocol.RequiresToBeNotNullEnity();

            return _mapper.Map<Protocol>(protocol);
        }

        public async Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            var protocols = await _context.Protocols.Where(p => p.ProductId == productId).ToListAsync();

            return _mapper.Map<IEnumerable<Protocol>>(protocols);
        }

        public async Task<IEnumerable<Protocol>> GetTemplateProtocols()
        {
            var protocols = await _context.Protocols.Where(p =>p.IsTemplate == true).ToListAsync();

            return _mapper.Map<IEnumerable<Protocol>>(protocols);
        }
    }
}
