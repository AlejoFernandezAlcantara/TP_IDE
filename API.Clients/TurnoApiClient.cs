using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class TurnoApiClient
    {
        private readonly HttpClient _client;

        public TurnoApiClient(HttpClient client)
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

        public async Task<List<TurnoDTO>?> GetAllAsync()
        {
            var r = await _client.GetAsync("turnos");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<TurnoDTO>>() : null;
        }

        public async Task<TurnoDTO?> GetByCodigoAsync(int codigo)
        {
            var r = await _client.GetAsync($"turnos/{codigo}");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<TurnoDTO>() : null;
        }

        public async Task<List<TurnoDTO>?> GetByOdontologoAsync(string matricula)
        {
            var r = await _client.GetAsync($"turnos/odontologo/{matricula}");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<TurnoDTO>>() : null;
        }

        public async Task<bool> CrearAsync(TurnoDTO dto)
            => (await _client.PostAsJsonAsync("turnos", dto)).IsSuccessStatusCode;

        public async Task<bool> ActualizarAsync(TurnoDTO dto)
            => (await _client.PutAsJsonAsync("turnos", dto)).IsSuccessStatusCode;

        public async Task<bool> EliminarAsync(int codigo)
            => (await _client.DeleteAsync($"turnos/{codigo}")).IsSuccessStatusCode;
    }
}   