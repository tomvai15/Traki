using SectionDomain = Traki.Domain.Models.Section.Section;

namespace Traki.Domain.Models
{
    public class Protocol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSigned { get; set; }
        public string CreationDate { get; set; }
        public bool IsCompleted { get; set; }
        public int? SignerId { get; set; }
        public User? Signer { get; set; }
        public string? ReportName { get; set; }
        public string? EnvelopeId { get; set; }
        public bool IsTemplate { get; set; }
        public int? ProductId { get; set; }
        public List<SectionDomain> Sections { get; set; }

        public Protocol CreateTemplate()
        {
            return new Protocol
            {
                Id = Id,
                Name = Name,
                CreationDate = CreationDate,
                IsTemplate = true,
                Sections = new List<SectionDomain>()
            };
        }

        public SectionDomain GetSection(int sectionId)
        {
            return Sections.FirstOrDefault(s => s.Id == sectionId);
        }

        public void AddSection(SectionDomain section)
        {
            Sections.Add(section);
        }

        public void UpdateSection(SectionDomain section)
        {
            var sectionToUpdate = GetSection(section.Id);

            Sections.Remove(sectionToUpdate);
            Sections.Add(section);
        }

        public void DeleteSection(int sectionId)
        {
            var sectionToRemove = GetSection(sectionId);
            Sections.Remove(sectionToRemove);
        }
    }
}
