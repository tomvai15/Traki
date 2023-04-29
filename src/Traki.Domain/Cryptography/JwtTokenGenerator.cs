using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Traki.Domain.Constants;
using Traki.Domain.Exceptions;
using Traki.Domain.Settings;

namespace Traki.Domain.Cryptography
{
    public interface IJwtTokenGenerator
    {
        string GenerateJWTToken(IEnumerable<Claim> claims);
        int GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
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

        public int GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_securitySettings.Secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            try
            {
                var principal = _securityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                var claim = principal.Identities.First().Claims.FirstOrDefault(x => x.Type == Claims.UserId);

                return int.Parse(claim.Value);

            } 
            catch(Exception ex)
            {
                throw new UnauthorizedException();
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
