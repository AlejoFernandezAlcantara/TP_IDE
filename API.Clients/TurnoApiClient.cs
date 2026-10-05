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

        public async Task<(bool Ok, string? Error)> CrearAsync(TurnoDTO dto)
            => await ResultadoAsync(await _client.PostAsJsonAsync("turnos", dto), "No se pudo crear el turno.");

        public async Task<bool> ActualizarAsync(TurnoDTO dto)
            => (await _client.PutAsJsonAsync("turnos", dto)).IsSuccessStatusCode;

        public async Task<(bool Ok, string? Error)> EliminarAsync(int codigo)
            => await ResultadoAsync(await _client.DeleteAsync($"turnos/{codigo}"), "No se pudo eliminar el turno.");

        // Si quien reserva es un paciente, el WebAPI ignora pacienteId y usa el del token
        public async Task<(bool Ok, string? Error)> ReservarAsync(int codigo, int pacienteId)
            => await ResultadoAsync(
                await _client.PostAsJsonAsync($"turnos/{codigo}/reservar", new { PacienteId = pacienteId }),
                "No se pudo reservar el turno.");

        public async Task<(bool Ok, string? Error)> CancelarReservaAsync(int codigo)
            => await ResultadoAsync(
                await _client.PostAsync($"turnos/{codigo}/cancelar-reserva", null),
                "No se pudo cancelar la reserva.");
        private static async Task<(bool Ok, string? Error)> ResultadoAsync(HttpResponseMessage r, string mensajePorDefecto)
        {
            if (r.IsSuccessStatusCode)
                return (true, null);

            if (r.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return (false, "No tenés permiso para realizar esta acción.");

            if (r.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return (false, "Tu sesión expiró. Volvé a iniciar sesión.");

            try
            {
                var body = await r.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (body != null && body.TryGetValue("error", out var msg) && !string.IsNullOrWhiteSpace(msg))
                    return (false, msg);
            }
            catch
            {
            }

            return (false, mensajePorDefecto);
        }
    }
}   