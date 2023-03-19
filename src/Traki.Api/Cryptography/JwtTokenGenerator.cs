using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Traki.Api.Settings;

namespace Traki.Api.Cryptography
{
    public interface IJwtTokenGenerator
    {
        string GenerateJWTToken(IEnumerable<Claim> claims);
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly SecuritySettings _securitySettings;
        private readonly SecurityTokenHandler _securityTokenHandler;

        public JwtTokenGenerator(IOptions<SecuritySettings> securitySettings, SecurityTokenHandler securityTokenHandler)
        {
            _securitySettings = securitySettings.Value;
            _securityTokenHandler = securityTokenHandler;
        }

        public string GenerateJWTToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = _securityTokenHandler.CreateToken(tokenDescriptor);

            var tokenString = _securityTokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
