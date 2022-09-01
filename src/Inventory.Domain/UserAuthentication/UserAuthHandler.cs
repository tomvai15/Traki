using Inventory.Domain.Token;
using Inventory.Domain.UserAuthentication.Login;

namespace Inventory.Domain.UserAuthentication
{
    public interface IUserAuthHandler
    {
        LoginResponse LoginUser(LoginRequest loginRequest)
    }
    public class UserAuthHandler : IUserAuthHandler
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public UserAuthHandler(IJwtTokenGenerator jwtTokenGenerator)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        public LoginResponse LoginUser(LoginRequest loginRequest)
        {

            User user = new User
            {
                Id = 1,
                Role = "Admin"
            };
            string token = jwtTokenGenerator.GenerateToken(user);

            return new LoginResponse
            {
                Token = $"Bearer {token}"
            };
        }
    }
}
