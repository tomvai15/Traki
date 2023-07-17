using Traki.Domain.Extensions;

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
        public List<Section> Sections { get; set; }

        public void AddSection(Section sectionToAdd)
        {
            var existingSection = Sections.First(x=> x.Id== sectionToAdd.Id);
            if (existingSection != null)
            {
                throw new InvalidOperationException("Cannot add section with same id");
            }

            Sections.Add(sectionToAdd);
        }

        public void DeleteSection(int sectionId)
        {
            var existingSection = Sections.First(x => x.Id == sectionId);
            existingSection.RequiresToBeNotNullEnity();
            Sections.Remove(existingSection);
        }

        public void Update(Section sectionToUpdate)
        {
            var existingSection = Sections.First(x => x.Id == sectionToUpdate.Id)                           ;
            Sections.Remove(existingSection);
            Sections.Add(sectionToUpdate);
        }
    }
}
