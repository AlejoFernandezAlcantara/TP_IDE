using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class PracticaApiClient
    {
        private readonly HttpClient _client;

        public PracticaApiClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("http://localhost:5232/api/");
        }

        public void SetToken(string? token)
        {
            _client.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<PracticaDTO>?> GetAllAsync()
        {
            var r = await _client.GetAsync("practicas");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<PracticaDTO>>() : null;
        }

        public async Task<(bool Ok, string? Error)> CrearAsync(PracticaDTO dto) =>
            await Resultado(await _client.PostAsJsonAsync("practicas", dto));

        public async Task<(bool Ok, string? Error)> ActualizarAsync(PracticaDTO dto) =>
            await Resultado(await _client.PutAsJsonAsync("practicas", dto));

        public async Task<(bool Ok, string? Error)> EliminarAsync(int codigo) =>
            await Resultado(await _client.DeleteAsync($"practicas/{codigo}"));

        private record ErrorResponse(string? Error);

        private static async Task<(bool Ok, string? Error)> Resultado(HttpResponseMessage r)
        {
            if (r.IsSuccessStatusCode) return (true, null);
            try
            {
                var body = await r.Content.ReadFromJsonAsync<ErrorResponse>();
                return (false, body?.Error ?? $"Error {(int)r.StatusCode}");
            }
            catch
            {
                return (false, $"Error {(int)r.StatusCode}");
            }
        }
    }
}