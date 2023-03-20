using Traki.Api.Services.Docusign.models;

namespace Traki.Api.Services.Docusign
{
    public interface IDocuSignService
    {
        Task<OAuthResponse> GetAccessToken(string code);
        Task<DocuSignUserInfo> GetUserInformation(string accessToken);
    }
}
