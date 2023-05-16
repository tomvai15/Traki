using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using Traki.Domain.Repositories;
using Traki.Domain.Services;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Settings;

namespace Traki.Domain.Providers
{
    public interface IAccessTokenProvider
    {
        Task<string> GetAccessToken();
        Task AddAccessToken(OAuthResponse oAuthResponse);
    }
    public class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly IClaimsProvider _claimsProvider;
        private readonly IDocuSignService _docuSignService;
        private readonly IMemoryCache _memoryCache;
        private readonly IUsersRepository _usersRepository;
        private readonly SecuritySettings _securitySettings;

        public AccessTokenProvider(IClaimsProvider claimsProvider, IDocuSignService docuSignService, IMemoryCache memoryCache, IUsersRepository usersRepository, IOptions<SecuritySettings> securitySettings)
        {
            _claimsProvider = claimsProvider;
            _docuSignService = docuSignService;
            _memoryCache = memoryCache;
            _usersRepository = usersRepository;
            _securitySettings = securitySettings.Value;
        }

        public async Task AddAccessToken(OAuthResponse oAuthResponse)
        {
            if (!_claimsProvider.TryGetUserId(out int userId))
            {
                throw new ArgumentException();
            }

            var user = await _usersRepository.GetUserById(userId);

            _memoryCache.Set(userId, oAuthResponse.AccessToken, DateTimeOffset.Now.AddHours(6));

            string encryptedRefreshToken = EncryptString(oAuthResponse.RefreshToken);
            user.EncryptedRefreshToken = encryptedRefreshToken;
            await _usersRepository.UpdateUser(user);
        }

        public async Task<string> GetAccessToken()
        {
            if (!_claimsProvider.TryGetUserId(out int userId))
            {
                throw new ArgumentException();
            }

            if (_memoryCache.TryGetValue<string>(userId, out string accessToken))
            {
                return accessToken;
            }

            var user = await _usersRepository.GetUserById(userId);

            if (user == null || user.EncryptedRefreshToken == null)
            {
                throw new ArgumentException();
            }

            string decryptedRefreshToken = DecryptString(user.EncryptedRefreshToken);
            var response =  await _docuSignService.GetAccessTokenUsingRefreshToken(decryptedRefreshToken);
            _memoryCache.Set(userId, response.AccessToken);

            string encryptedRefreshToken = EncryptString(response.RefreshToken);
            user.EncryptedRefreshToken = encryptedRefreshToken;
            await _usersRepository.UpdateUser(user);
            return response.AccessToken;
        }

        private string EncryptString(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_securitySettings.Key).Take(32).ToArray();
                aes.IV = Encoding.UTF8.GetBytes(_securitySettings.InitializationVector).Take(16).ToArray();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedBytes;
                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cs.FlushFinalBlock();

                    encryptedBytes = ms.ToArray();
                }

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        private string DecryptString(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_securitySettings.Key).Take(32).ToArray();
                aes.IV = Encoding.UTF8.GetBytes(_securitySettings.InitializationVector).Take(16).ToArray();

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(cipherTextBytes, 0, cipherTextBytes.Length);
                    cs.FlushFinalBlock();

                    byte[] decryptedBytes = ms.ToArray();
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}
