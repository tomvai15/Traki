using Traki.Api.Contracts.Section;

namespace Traki.Api.Contracts.Protocol
{
    public class UpdateProtocolRequest
    {
        public ProtocolDto Protocol { get; set; }
        public IEnumerable<SectionBaseDto> Sections { get; set; }
    }
}
