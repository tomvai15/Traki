using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Contracts.Protocol
{
    public record ProtocolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreationDate { get; set; }
        public bool IsCompleted { get; set; }
        public int? SignerId { get; set; }
        public AuthorDto? Signer { get; set; }
        public bool IsSigned { get; set; }
        public string? ReportName { get; set; }
        public bool IsTemplate { get; set; }
        public ICollection<SectionBaseDto> Sections { get; set; }
    }
}
