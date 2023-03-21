namespace Traki.Api.Contracts.Auth
{
    public class LoginOAuthRequest
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
