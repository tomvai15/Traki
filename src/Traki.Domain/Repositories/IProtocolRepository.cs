using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IProtocolRepository
    {
        Task UpdateProtocol(Protocol protocol);
        Task<Protocol> GetProtocol(int protocolId);
        Task<IEnumerable<Protocol>> GetTemplateProtocols();
        Task CreateProtocol(Protocol protocol);
        Task<Protocol> UpdateProtocol();
    }
}
