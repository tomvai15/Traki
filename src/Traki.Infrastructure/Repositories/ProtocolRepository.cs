using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            _context.Add(protocolEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<Protocol>(protocolEntity);
        }

        public Task DeleteProtocol(int protocolId)
        {
            throw new NotImplementedException();
        }

        public async Task<Protocol> GetProtocol(int protocolId)
        {
            var protocol = _context.Protocols
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.TextInput)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.Question)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.MultipleChoice)
                                                    .ThenInclude(x => x.Options)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Table)
                                            .ThenInclude(x => x.TableRows)
                                                .ThenInclude(x => x.RowColumns)
                                    .First(x => x.Id == protocolId);
                
            var a = _mapper.Map<Protocol>(protocol);

            return a;
        }

        public Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Protocol>> GetTemplateProtocols()
        {
            var protocols = await _context.Protocols.Where(p => p.IsTemplate == true).ToListAsync();

            return _mapper.Map<IEnumerable<Protocol>>(protocols);
        }

        public async Task<Protocol> UpdateProtocol(Protocol protocol)
        {
            var protocolEntity = _mapper.Map<ProtocolEntity>(protocol);
            _context.Protocols.Update(protocolEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<Protocol>(protocolEntity);
        }
    }
}
