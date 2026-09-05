using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http.Headers;
using WindowsForms.Auth;

namespace WindowsForms.ApiClients
{
    public abstract class BaseApiClient
    {
        private const string BaseUrl = "http://localhost:5232/api/";

        protected static async Task<HttpClient> CreateHttpClientAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            await AddAuthorizationHeaderAsync(client);
            return client;
        }

        protected static async Task AddAuthorizationHeaderAsync(HttpClient client)
        {
            var authService = AuthServiceProvider.Instance;
            await authService.CheckTokenExpirationAsync();
            var token = await authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected static async Task HandleUnauthorizedResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var authService = AuthServiceProvider.Instance;
                await authService.LogoutAsync();
                throw new UnauthorizedAccessException("Su sesión ha expirado.");
            }
        }
    }
}