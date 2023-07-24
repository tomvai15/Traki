using Traki.Api.Contracts.Section;

namespace Traki.Api.Contracts.Protocol
{
    public class UpdateProtocolRequest
    {
        public ProtocolDto Protocol { get; set; }
        public IEnumerable<SectionDto> Sections { get; set; }
    }
}
