namespace Traki.Api.Contracts.Auth
{
    public class AuthorisationCodeRequest
    {
        public string State { get; set; }
        public bool LoginAsAdmin { get; set; }
    }
}
