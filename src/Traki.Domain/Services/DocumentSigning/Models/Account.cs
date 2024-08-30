using System.Text.Json.Serialization;

namespace Traki.Domain.Services.Docusign.Models
{
    public class Account
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("is_default")]
        public string IsDefault { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("base_uri")]
        public string BaseUri { get; set; }
    }
}
