using Traki.Domain.Services.Docusign.models;

namespace Traki.Domain.Services.Docusign
{
    public interface IDocuSignService
    {
        Task<string> GetAuthorisationCodeRequest();
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
