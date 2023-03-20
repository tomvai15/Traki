using Traki.Api.Services.Docusign.models;

namespace Traki.Api.Services.Docusign
{
    public interface IDocuSignService
    {
        Task<OAuthResponse> GetAccessToken(string code);
        Task<DocuSignUserInfo> GetUserInformation(string accessToken);

        // TODO: refactor
        (string, string) SendEnvelopeForEmbeddedSigning(
            string signerEmail,
            string signerName,
            string signerClientId,
            string accessToken,
            string basePath,
            string accountId,
            string docPdf,
            string returnUrl,
            string pingUrl = null);
    }
}
