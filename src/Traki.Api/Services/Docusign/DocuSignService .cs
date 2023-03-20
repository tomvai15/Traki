using DocuSign.eSign.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Traki.Api.Contracts.Product;
using Traki.Api.Services.Docusign.models;
using Traki.Api.Settings;

namespace Traki.Api.Services.Docusign
{
    public class DocuSignService : IDocuSignService
    {
        private readonly DocuSignClient _docuSignClient = new();
        private readonly DocuSignSettings _docuSignSettings;
        private readonly HttpClient _httpClient;

        public DocuSignService(IOptions<DocuSignSettings> docuSignOptions, HttpClient httpClient)
        {
            _docuSignSettings = docuSignOptions.Value;
            _httpClient = httpClient;
        }

        public async Task<OAuthResponse> GetAccessToken(string code)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;

            string s = _docuSignSettings.ClientId + ":" + _docuSignSettings.ClientSecret;
            string text = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));

            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>( "code", code ),
            };

            var httpContent = new FormUrlEncodedContent(data);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", text);

            var response = await _httpClient.PostAsync(_docuSignSettings.TokenEndpoint, httpContent);

            var content = await response.Content.ReadAsStringAsync();
            var oauthResponse = JsonConvert.DeserializeObject<OAuthResponse>(content);

            return oauthResponse;
        }

        public async Task<DocuSignUserInfo> GetUserInformation(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(_docuSignSettings.UserInformationEndpoint);

            var content = await response.Content.ReadAsStringAsync();
            var userInformation = JsonConvert.DeserializeObject<DocuSignUserInfo>(content);

            return userInformation;
        }
    }
}
