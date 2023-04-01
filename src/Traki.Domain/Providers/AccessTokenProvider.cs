using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Domain.Services;
using Traki.Domain.Services.Docusign;

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

        public AccessTokenProvider(IClaimsProvider claimsProvider, IDocuSignService docuSignService, IMemoryCache memoryCache, IUsersRepository usersRepository)
        {
            _claimsProvider = claimsProvider;
            _docuSignService = docuSignService;
            _memoryCache = memoryCache;
            _usersRepository = usersRepository;
        }

        public async Task AddAccessToken(OAuthResponse oAuthResponse)
        {
            if (!_claimsProvider.TryGetUserId(out int userId))
            {
                throw new ArgumentException();
            }

            var user = await _usersRepository.GetUserById(userId);

            _memoryCache.Set(userId, oAuthResponse.AccessToken);
            user.EncryptedRefreshToken = oAuthResponse.RefreshToken;
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

            // Todo: decrypt....
            if (user == null || user.EncryptedRefreshToken == null)
            {
                throw new ArgumentException();
            }

            var response =  await _docuSignService.GetAccessTokenUsingRefreshToken(user.EncryptedRefreshToken);
            _memoryCache.Set(userId, response.AccessToken);

            user.EncryptedRefreshToken = response.RefreshToken;
            await _usersRepository.UpdateUser(user);
            return response.AccessToken;
        }
    }
}
