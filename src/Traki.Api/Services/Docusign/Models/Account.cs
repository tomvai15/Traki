using Newtonsoft.Json;

namespace Traki.Api.Services.Docusign.Models
{
    public class Account
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("is_default")]
        public string IsDefault { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("base_uri")]
        public string BaseUri { get; set; }
    }
}
