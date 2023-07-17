using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Entities
{
    public class ProtocolEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSigned { get; set; }
        public bool IsCompleted { get; set; }
        public string CreationDate { get; set; }
        public int? SignerId { get; set; }
        public UserEntity? Signer  { get; set; }
        public string? ReportName { get; set; }
        public string? EnvelopeId { get; set; }
        public bool IsTemplate { get; set; }
        public int? ProductId { get; set; }
        public ProductEntity? Product { get; set; }
        public ICollection<ChecklistEntity> Checklists { get; set; }
        public ICollection<TableEntity> Tables { get; set; }
    }
}
