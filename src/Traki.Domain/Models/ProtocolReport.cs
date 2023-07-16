namespace Traki.Domain.Models
{
    public class ProtocolReport
    {
        public string ProtocolName { get; set; }
        public string CompanyLogoBase64 { get; set; }
        public Company Company { get; set; }
        public Project Project { get; set; }
        public Product Product { get; set; }
        public Protocol Protocol { get; set; }
        public List<Models.Section> Sections { get; set; }
        public List<ItemImage> ItemImages { get; set; }
    }
}
