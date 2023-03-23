using Newtonsoft.Json;
using Traki.Domain.Services.Docusign.Models;

namespace Traki.Domain.Services.Docusign.models
{
    public class DocuSignUserInfo
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("family_name")]
        public string FamilName { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("accounts")]
        public IEnumerable<Account> Accounts { get; set; }
    }
}
