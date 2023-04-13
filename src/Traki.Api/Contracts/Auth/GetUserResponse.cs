namespace Traki.Api.Contracts.Auth
{
    public class GetUserInfoResponse
    {
        public UserInfoDto User { get; set; }
        public bool LoggedInDocuSign { get; set; }
    }
}
