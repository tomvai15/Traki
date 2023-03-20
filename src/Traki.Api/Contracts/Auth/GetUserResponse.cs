namespace Traki.Api.Contracts.Auth
{
    public class GetUserResponse
    {
        public UserDto User { get; set; }
        public bool LoggedInDocuSign { get; set; }
    }
}
