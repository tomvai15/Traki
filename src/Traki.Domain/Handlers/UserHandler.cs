﻿using Azure.Core;
using Microsoft.Extensions.Options;
using Traki.Domain.Constants;
using Traki.Domain.Cryptography;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Docusign;
using Traki.Domain.Services.Email;

namespace Traki.Domain.Handlers
{
    public interface IUserHandler
    {
        Task CreateUser(User user);
    }
    public class UserHandler : IUserHandler
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHasherAdapter _hasherAdapter;
        private readonly IEmailService _emailService;
        private readonly IDocuSignService _docuSignService;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly WebSettings _webSettings;

        public UserHandler(IUsersRepository usersRepository, IHasherAdapter hasherAdapter, IEmailService emailService, IOptions<WebSettings> webSettings, IDocuSignService docuSignService, IAccessTokenProvider accessTokenProvider)
        {
            _usersRepository = usersRepository;
            _hasherAdapter = hasherAdapter;
            _emailService = emailService;
            _webSettings = webSettings.Value;
            _docuSignService = docuSignService;
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task CreateUser(User user)
        {
            var userByEmail = await _usersRepository.GetUserByEmail(user.Email);

            if (userByEmail != null)
            {
                throw new InvalidOperationException();
            }

            var registerId = Guid.NewGuid().ToString();
            var tempPassword = Guid.NewGuid().ToString();
            var hashedPassword = _hasherAdapter.HashText(tempPassword);

            user.RegisterId = registerId;
            user.HashedPassword = hashedPassword;
            user.Status = UserStatus.Created;

            await _usersRepository.AddNewUser(user);

            string link = $"{_webSettings.Url}/auth/register?code={tempPassword}&acc={registerId}";
            await _emailService.SendEmail(user.Email, "Activate account", $"Activate account via link: {link}");

            //string accessToken = await _accessTokenProvider.GetAccessToken();
            //var userInfo = await _docuSignService.GetUserInformation(accessToken);
           // await _docuSignService.CreateUser(userInfo, accessToken, user.Name, user.Surname, user.Email);
        }
    }
}
