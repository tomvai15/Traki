using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Traki.Domain.Constants;
using Traki.Domain.Services;
using Traki.Domain.Services.DocumentSigning.Models;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Docusign.models;

namespace Traki.Infrastructure.Services
{
    public class DocuSignService : IDocuSignService
    {
        private readonly WebSettings _webSettings;
        private readonly DocuSignSettings _docuSignSettings;
        private readonly HttpClient _httpClient;

        public DocuSignService(IOptions<WebSettings> webSettings, IOptions<DocuSignSettings> docuSignOptions, HttpClient httpClient)
        {
            _docuSignSettings = docuSignOptions.Value;
            _httpClient = httpClient;
            _webSettings = webSettings.Value;
        }

        public async Task<OAuthResponse> GetAccessTokenUsingCode(string code)
        {
            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>( "code", code ),
            };

            return await GetAccessToken(data);
        }

        public async Task<OAuthResponse> GetAccessTokenUsingRefreshToken(string refreshToken)
        {
            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>( "refresh_token", refreshToken ),
            };

            return await GetAccessToken(data);
        }

        public async Task<DocuSignUserInfo> GetUserInformation(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(_docuSignSettings.UserInformationEndpoint);

            var content = await response.Content.ReadAsStringAsync();
            var userInformation = JsonConvert.DeserializeObject<DocuSignUserInfo>(content);

            return userInformation;
        }

        public async Task<Stream> GetDocument(string accessToken, string basePath, string accountId, string envelopeId, string documentId)
        {
            var docuSignClient = new DocuSignClient(basePath);
            docuSignClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

            EnvelopesApi envelopesApi = new EnvelopesApi(docuSignClient);
            var stream = await envelopesApi.GetDocumentAsync(accountId, envelopeId, documentId);

            return stream;
        }

        public async Task<Stream> GetPdfDocument(DocuSignUserInfo docuSignUserInfo, string envelopeId, string accessToken)
        {
            string accountId = docuSignUserInfo.Accounts.First().AccountId;
            string documentId = "3";
            string basePath = docuSignUserInfo.Accounts.First().BaseUri + "/restapi";

            var docuSignClient = new DocuSignClient(basePath);
            docuSignClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

            EnvelopesApi envelopesApi = new EnvelopesApi(docuSignClient);
            var stream = await envelopesApi.GetDocumentAsync(accountId, envelopeId, documentId);

            return stream;
        }

        public async Task<SignDocumentResult> CreateDocumentSigningRedirectUri(DocuSignUserInfo docuSignUserInfo, string accessToken, string docPdf, string state)
        {
            string returnUrl =  _webSettings.Url + "/signvalidation";
            const string pathEnd = "/restapi";
            var signerEmail = docuSignUserInfo.Email;
            var signerName = docuSignUserInfo.Name;
            var account = docuSignUserInfo.Accounts.First();
            var accountId = account.AccountId;
            string basePath = account.BaseUri + pathEnd;
            const string signerClientId = "1000";

            EnvelopeDefinition envelope = MakeEnvelope(signerEmail, signerName, signerClientId, docPdf);

            var docuSignClient = new DocuSignClient(basePath);
            docuSignClient.Configuration.DefaultHeader.Add("Authorization", "Bearer " + accessToken);

            EnvelopesApi envelopesApi = new EnvelopesApi(docuSignClient);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelope);
            string envelopeId = results.EnvelopeId;

            RecipientViewRequest viewRequest = MakeRecipientViewRequest(signerEmail, signerName, returnUrl, signerClientId, state, null);

            ViewUrl results1 = envelopesApi.CreateRecipientView(accountId, envelopeId, viewRequest);
            string redirectUrl = results1.Url;

            return new SignDocumentResult { EnvelopeId = envelopeId, RedirectUri = redirectUrl };
        }

        private static RecipientViewRequest MakeRecipientViewRequest(string signerEmail, string signerName, string returnUrl, string signerClientId, string state, string pingUrl = null)
        {
            RecipientViewRequest viewRequest = new RecipientViewRequest();

            viewRequest.ReturnUrl = returnUrl + "?state=" + state;

            viewRequest.AuthenticationMethod = "none";

            viewRequest.Email = signerEmail;
            viewRequest.UserName = signerName;
            viewRequest.ClientUserId = signerClientId;

            if (pingUrl != null)
            {
                viewRequest.PingFrequency = "600"; // seconds
                viewRequest.PingUrl = pingUrl; // optional setting
            }

            return viewRequest;
        }

        private static EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string signerClientId, string docPdf)
        {
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition();
            envelopeDefinition.EmailSubject = "Please sign this document";
            Document doc1 = new Document();


            doc1.DocumentBase64 = docPdf;
            doc1.Name = "Protocol Report"; 
            doc1.FileExtension = "pdf";
            doc1.DocumentId = "3";

            envelopeDefinition.Documents = new List<Document> { doc1 };

            Signer signer1 = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = "1",
            };

            SignHere signHere1 = new SignHere
            {
                AnchorString = "/sn1/",
                AnchorUnits = "pixels",
                AnchorXOffset = "10",
                AnchorYOffset = "20",
            };

            Tabs signer1Tabs = new Tabs
            {
                SignHereTabs = new List<SignHere> { signHere1 },
            };
            signer1.Tabs = signer1Tabs;

            Recipients recipients = new Recipients
            {
                Signers = new List<Signer> { signer1 },
            };
            envelopeDefinition.Recipients = recipients;

            envelopeDefinition.Status = "sent";

            return envelopeDefinition;
        }

        public async Task<string> GetAuthorisationCodeRequest(string state)
        {
            string redirectUrl =  _webSettings.Url  + "/checkoauth";
            return $"https://account-d.docusign.com/oauth/auth?response_type=code&scope=signature&client_id={_docuSignSettings.ClientId}&redirect_uri={redirectUrl}&state={state}";
        }

        private async Task<OAuthResponse> GetAccessToken(KeyValuePair<string, string>[] requestData)
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = HttpMethod.Post;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", CreateAuthCode());

            var httpContent = new FormUrlEncodedContent(requestData);

            var response = await _httpClient.PostAsync(_docuSignSettings.TokenEndpoint, httpContent);

            var content = await response.Content.ReadAsStringAsync();
            var oauthResponse = JsonConvert.DeserializeObject<OAuthResponse>(content);

            return oauthResponse;
        }

        private string CreateAuthCode()
        {
            string authCode = _docuSignSettings.ClientId + ":" + _docuSignSettings.ClientSecret;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(authCode));
        }
    }
}
