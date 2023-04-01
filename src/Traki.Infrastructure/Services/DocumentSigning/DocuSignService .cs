using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Traki.Domain.Models;
using Traki.Domain.Services;
using Traki.Domain.Services.DocumentSigning.Models;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Docusign.models;
using static DocuSign.eSign.Client.Auth.OAuth;

namespace Traki.Infrastructure.Services.Docusign
{
    public class DocuSignService : IDocuSignService
    {
        private readonly DocuSignSettings _docuSignSettings;
        private readonly HttpClient _httpClient;

        public DocuSignService(IOptions<DocuSignSettings> docuSignOptions, HttpClient httpClient)
        {
            _docuSignSettings = docuSignOptions.Value;
            _httpClient = httpClient;
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

        public async Task<SignDocumentResult> CreateDocumentSigningRedirectUri(DocuSignUserInfo docuSignUserInfo, string accessToken, string docPdf, string returnUrl, string state)
        {
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
            // Data for this method
            // signerEmail
            // signerName
            // dsPingUrl -- class global
            // signerClientId -- class global
            // dsReturnUrl -- class global
            RecipientViewRequest viewRequest = new RecipientViewRequest();

            // Set the url where you want the recipient to go once they are done signing
            // should typically be a callback route somewhere in your app.
            // The query parameter is included as an example of how
            // to save/recover state information during the redirect to
            // the DocuSign signing ceremony. It's usually better to use
            // the session mechanism of your web framework. Query parameters
            // can be changed/spoofed very easily.
            viewRequest.ReturnUrl = returnUrl + "?state=" + state;

            // How has your app authenticated the user? In addition to your app's
            // authentication, you can include authenticate steps from DocuSign.
            // Eg, SMS authentication
            viewRequest.AuthenticationMethod = "none";

            // Recipient information must match embedded recipient info
            // we used to create the envelope.
            viewRequest.Email = signerEmail;
            viewRequest.UserName = signerName;
            viewRequest.ClientUserId = signerClientId;

            // DocuSign recommends that you redirect to DocuSign for the
            // Signing Ceremony. There are multiple ways to save state.
            // To maintain your application's session, use the pingUrl
            // parameter. It causes the DocuSign Signing Ceremony web page
            // (not the DocuSign server) to send pings via AJAX to your
            // app,
            // NOTE: The pings will only be sent if the pingUrl is an https address
            if (pingUrl != null)
            {
                viewRequest.PingFrequency = "600"; // seconds
                viewRequest.PingUrl = pingUrl; // optional setting
            }

            return viewRequest;
        }

        private static EnvelopeDefinition MakeEnvelope(string signerEmail, string signerName, string signerClientId, string docPdf)
        {
            // Data for this method
            // signerEmail
            // signerName
            // signerClientId -- class global
            // Config.docPdf

            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition();
            envelopeDefinition.EmailSubject = "Please sign this document";
            Document doc1 = new Document();


            doc1.DocumentBase64 = docPdf;
            doc1.Name = "Lorem Ipsum"; // can be different from actual file name
            doc1.FileExtension = "pdf";
            doc1.DocumentId = "3";

            // The order in the docs array determines the order in the envelope
            envelopeDefinition.Documents = new List<Document> { doc1 };

            // Create a signer recipient to sign the document, identified by name and email
            // We set the clientUserId to enable embedded signing for the recipient
            // We're setting the parameters via the object creation
            Signer signer1 = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                ClientUserId = signerClientId,
                RecipientId = "1",
            };

            // Create signHere fields (also known as tabs) on the documents,
            // We're using anchor (autoPlace) positioning
            //
            // The DocuSign platform searches throughout your envelope's
            // documents for matching anchor strings.
            SignHere signHere1 = new SignHere
            {
                AnchorString = "/sn1/",
                AnchorUnits = "pixels",
                AnchorXOffset = "10",
                AnchorYOffset = "20",
            };

            // Tabs are set per recipient / signer
            Tabs signer1Tabs = new Tabs
            {
                SignHereTabs = new List<SignHere> { signHere1 },
            };
            signer1.Tabs = signer1Tabs;

            // Add the recipient to the envelope object
            Recipients recipients = new Recipients
            {
                Signers = new List<Signer> { signer1 },
            };
            envelopeDefinition.Recipients = recipients;

            // Request that the envelope be sent by setting |status| to "sent".
            // To request that the envelope be created as a draft, set to "created"
            envelopeDefinition.Status = "sent";

            return envelopeDefinition;
        }

        public async Task<string> GetAuthorisationCodeRequest(string state)
        {
            const string redirectUrl = "http://localhost:3000/checkoauth";
            return $"https://account-d.docusign.com/oauth/auth?response_type=code&scope=signature&client_id={_docuSignSettings.ClientId}&redirect_uri={redirectUrl}&state={state}";
        }

        public Task<Stream> GetPdfDocument(string accessToken, string basePath, string accountId, string envelopeId, string documentId)
        {
            throw new NotImplementedException();
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
