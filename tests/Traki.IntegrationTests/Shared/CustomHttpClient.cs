using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;

namespace Traki.IntegrationTests.Shared
{
    public class CustomHttpClient
    {
        public readonly HttpClient _httpClient;

        public CustomHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void AddBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<Response<TResponse>> Post<TRequest, TResponse>(string path,TRequest requestBody)
        {
            return await Send<TRequest, TResponse>(path, HttpMethod.Post, requestBody);
        }
        
        public async Task<Response<TResponse>> Get<TResponse>(string path)
        {
            return await Send<TResponse>(path, HttpMethod.Get);
        }

        public async Task<Response<TResponse>> Send<TRequest, TResponse>(string path, HttpMethod httpMethod, TRequest requestBody)
        {
            Uri uri = new Uri(path.TrimStart('/').TrimStart(), UriKind.Relative);
            using HttpRequestMessage httpRequestMessage = CreateHttpRequestMessage<TResponse>(httpMethod, uri);

            if (requestBody != null)
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            return await SendRequest<TResponse>(httpRequestMessage);
        }

        public async Task<Response<TResponse>> Send<TResponse>(string path, HttpMethod httpMethod)
        {
            Uri uri = new Uri(path.TrimStart('/').TrimStart(), UriKind.Relative);
            using HttpRequestMessage httpRequestMessage = CreateHttpRequestMessage<TResponse>(httpMethod, uri);

            return await SendRequest<TResponse>(httpRequestMessage);
        }


        public async Task<Response<TResponse>> SendRequest<TResponse>(HttpRequestMessage httpRequestMessage)
        {
            var response = await _httpClient.SendAsync(httpRequestMessage);

            string consent = await response.Content.ReadAsStringAsync();

            var responseResult = new Response<TResponse>
            {
                StatusCode = response.StatusCode,
                Message = "",
                Data = default(TResponse)
            };

            if (consent.IsNullOrEmpty())
            {
                return responseResult;
            }
            var responseData = JsonConvert.DeserializeObject<TResponse>(consent);

            responseResult.Data = responseData;

            return responseResult;
        }

        private HttpRequestMessage CreateHttpRequestMessage<TBody>(HttpMethod httpMethod, Uri relativePath)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Method = httpMethod;
            request.RequestUri = relativePath;
            return request;
        }
    }
}
