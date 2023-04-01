using Traki.Domain.Services.DocumentSigning.Models;
using Traki.Domain.Services.Docusign.models;

namespace Traki.Domain.Services.Docusign
{
    public interface IDocuSignService
    {
        Task<Stream> GetDocument(string accessToken, string basePath, string accountId, string envelopeId, string documentId);
        Task<Stream> GetPdfDocument(DocuSignUserInfo docuSignUserInfo, string envelopeId, string accessToken);
        Task<string> GetAuthorisationCodeRequest(string state);
        Task<OAuthResponse> GetAccessTokenUsingCode(string code);
        Task<OAuthResponse> GetAccessTokenUsingRefreshToken(string refreshToken);
        Task<DocuSignUserInfo> GetUserInformation(string accessToken);
        Task<SignDocumentResult> CreateDocumentSigningRedirectUri(DocuSignUserInfo docuSignUserInfo, string accessToken,string docPdf, string returnUrl, string state);
    }
}
