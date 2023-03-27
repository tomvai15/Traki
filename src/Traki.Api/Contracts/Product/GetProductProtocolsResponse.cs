using Traki.Api.Contracts.Protocol;

namespace Traki.Api.Contracts.Product
{
    public class GetProductProtocolsResponse
    {
        public IEnumerable<ProtocolDto> Protocols { get; set; }
    }
}
