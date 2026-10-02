using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class ReservaApiClient
    {
        private readonly HttpClient _client;

        public ReservaApiClient(HttpClient client)
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

        public async Task<List<ReservaDTO>?> GetAllAsync()
        {
            var r = await _client.GetAsync("reservas");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<ReservaDTO>>() : null;
        }

        public async Task<List<ReservaDTO>?> GetByPacienteAsync(int pacienteId)
        {
            var r = await _client.GetAsync($"reservas/paciente/{pacienteId}");
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<List<ReservaDTO>>() : null;
        }

        public async Task<bool> CrearAsync(ReservaDTO dto)
            => (await _client.PostAsJsonAsync("reservas", dto)).IsSuccessStatusCode;

        public async Task<bool> ActualizarAsync(ReservaDTO dto)
            => (await _client.PutAsJsonAsync("reservas", dto)).IsSuccessStatusCode;

        /*public async Task<bool> EliminarAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion)
        {
            var fecha = Uri.EscapeDataString(fechaCreacion.ToString("O"));
            var r = await _client.DeleteAsync($"reservas/{pacienteId}/{odontologoMatricula}?fechaCreacion={fecha}");
            return r.IsSuccessStatusCode;
        }*/
        public async Task<bool> EliminarAsync(int id)
         => (await _client.DeleteAsync($"reservas/{id}")).IsSuccessStatusCode;
    }
}