namespace Traki.Api.Contracts.Protocol
{
    public class GetProtocolsResponse
    {
        public IEnumerable<ProtocolDto> Protocols { get; set; }
    }
}
