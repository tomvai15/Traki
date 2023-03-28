namespace Traki.Domain.Models
{
    public class ProtocolReport
    {
        public string CompanyLogoBase64 { get; set; }
        public Company Company { get; set; }
        public Project Project { get; set; }
        public Product Product { get; set; }
        public Protocol Protocol { get; set; }
        public List<Section.Section> Sections { get; set; }
    }
}
