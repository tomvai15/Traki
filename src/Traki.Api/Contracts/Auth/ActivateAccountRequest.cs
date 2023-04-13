namespace Traki.Api.Contracts.Auth
{
    public class ActivateAccountRequest
    {
        public string RegisterId { get; set; }
        public string Code { get; set; }
        public string Password { get; set; }
    }
}
