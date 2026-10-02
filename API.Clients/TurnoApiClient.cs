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
        public async Task<ReservaDTO?> ReservarAsync(int codigo, int pacienteId)
        {
            // Si quien reserva es un paciente, el WebAPI ignora pacienteId y usa el del token
            var r = await _client.PostAsJsonAsync($"turnos/{codigo}/reservar", new { PacienteId = pacienteId });
            return r.IsSuccessStatusCode ? await r.Content.ReadFromJsonAsync<ReservaDTO>() : null;
        }

        public async Task<(bool Ok, string? Error)> CancelarReservaAsync(int codigo)
        {
            var r = await _client.PostAsync($"turnos/{codigo}/cancelar-reserva", null);
            if (r.IsSuccessStatusCode)
                return (true, null);

            try
            {
                var body = await r.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return (false, body != null && body.TryGetValue("error", out var msg) ? msg : null);
            }
            catch
            {
                return (false, null);
            }
        }
    }
}   