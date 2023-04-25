namespace Traki.Api.Contracts.Auth
{
    public class GetUserStateResponse
    {
        public UserInfoDto User { get; set; }
        public bool LoggedInDocuSign { get; set; }
    }
}
