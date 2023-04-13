namespace Traki.Api.Contracts.User
{
    public class GetUsersResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
