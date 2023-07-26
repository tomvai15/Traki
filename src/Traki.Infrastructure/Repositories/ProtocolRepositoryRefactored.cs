﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;

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
                                    .Include(x => x.Checklists)
                                        .ThenInclude(x => x.Questions)
                                    .Include(x => x.Checklists)
                                        .ThenInclude(x => x.TextInputs)
                                    .Include(x => x.Checklists)
                                        .ThenInclude(x => x.MultipleChoices)
                                            .ThenInclude(x => x.Options)
                                     .Include(x => x.Tables)
                                        .ThenInclude(x => x.TableRows)
                                            .ThenInclude(x => x.RowColumns)
                                    .First(x => x.Id == protocolId);


            var a = _mapper.Map<Protocol>(protocol);

            var b = _mapper.Map<ProtocolEntity>(a);

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

        public Task<Protocol> UpdateProtocol(Protocol protocol)
        {
            throw new NotImplementedException();
        }
    }
}
