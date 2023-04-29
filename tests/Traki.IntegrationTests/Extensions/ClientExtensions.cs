using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Traki.Api.Contracts.Auth;
using Traki.IntegrationTests.Shared;

namespace Traki.IntegrationTests.Extensions
{
    public static class ClientExtensions
    {
        public static async Task LoginAsProductManager(this CustomHttpClient client)
        {
            await client.AddJwtToken("vainoristomas@gmail.com", "password");
        }

        public static async Task AddJwtToken(this CustomHttpClient client, string email, string password)
        {
            string uri = "api/auth/jwt-login";

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var response = await client.Post<LoginRequest, LoginResponse>(uri, loginRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            client.AddBearerToken(response.Data.Token);
        }

        public static CustomHttpClient GetCustomHttpClient(this WebApplicationFactory<Program> factory)
        {
            return new CustomHttpClient(factory.CreateClient());
        }
    }
}
