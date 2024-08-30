using System.Text.Json.Serialization;
using Traki.Domain.Services.Docusign.Models;

namespace Traki.Domain.Services.Docusign.models
{
    public class DocuSignUserInfo
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("family_name")]
        public string FamilName { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("accounts")]
        public IEnumerable<Account> Accounts { get; set; }
    }
}
