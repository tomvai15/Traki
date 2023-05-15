namespace Traki.Domain.Services.Docusign
{
    public class DocuSignSettings
    {
        public static readonly string SectionName = "Docusign";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string UserInformationEndpoint { get; set; }
        public string AdminApiEndpoint { get; set; }
    }
}
