using System.Net.Http.Json;
using DTO;
using Applications.Services;

namespace API.Clients
{
    public class AuthApiClient
    {
        private readonly HttpClient _client;

        public AuthApiClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:5232/api/");
        }

        public async Task<LoginResponseDTO?> LoginAsync(string email, string password)
        {
            var request = new LoginRequestDTO { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync("auth/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
        }
    }
}