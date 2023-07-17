using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;

namespace Traki.Infrastructure.Repositories
{
    public class ProtocolRepositoryRefactored : IProtocolRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ProtocolRepositoryRefactored(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<Protocol> CreateProtocol(Protocol protocol)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProtocol(int protocolId)
        {
            throw new NotImplementedException();
        }

        public async Task<Protocol> GetProtocol(int protocolId)
        {
            var protocol = _context.Protocols
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Table)
                                            .ThenInclude(x => x.TableRows)
                                                .ThenInclude(x => x.RowColumns)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.Question)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.MultipleChoice)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.TextInput)
                                    .First(x => x.Id == protocolId);


            return _mapper.Map<Protocol>(protocol);
        }

        public Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Protocol>> GetTemplateProtocols()
        {
            throw new NotImplementedException();
        }

        public Task<Protocol> UpdateProtocol(Protocol protocol)
        {
            throw new NotImplementedException();
        }
    }
}
