using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class PacienteApiClient
    {
        private readonly HttpClient _client;

        public PacienteApiClient(HttpClient client)
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

        public async Task<List<PacienteDTO>?> GetAllAsync()
        {
            var response = await _client.GetAsync("pacientes");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<List<PacienteDTO>>();
        }

        public async Task<PacienteDTO?> GetByNroPacienteAsync(int nroPaciente)
        {
            var response = await _client.GetAsync($"pacientes/{nroPaciente}");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<PacienteDTO>();
        }

        public async Task<(bool Ok, string? Error)> CrearAsync(PacienteDTO paciente)
        {
            var response = await _client.PostAsJsonAsync("pacientes", paciente);
            return await ResultadoAsync(response, "No se pudo guardar el paciente.");
        }

        // eL AUTORREGISTRO NO ENVIA TOKEN
        public async Task<(bool Ok, string? Error)> RegistrarAsync(PacienteDTO paciente)
        {
            var response = await _client.PostAsJsonAsync("pacientes/registro", paciente);
            return await ResultadoAsync(response, "No se pudo completar el registro.");
        }

        public async Task<(bool Ok, string? Error)> ActualizarAsync(PacienteDTO paciente)
        {
            var response = await _client.PutAsJsonAsync("pacientes", paciente);
            return await ResultadoAsync(response, "No se pudo guardar el paciente.");
        }

        public async Task<bool> EliminarAsync(int nroPaciente)
        {
            var response = await _client.DeleteAsync($"pacientes/{nroPaciente}");
            return response.IsSuccessStatusCode;
        }

        // Toma el campo "error" de la respuesta de la API; si no hay, usa el mensaje por defecto.
        private static async Task<(bool Ok, string? Error)> ResultadoAsync(HttpResponseMessage response, string mensajePorDefecto)
        {
            if (response.IsSuccessStatusCode)
                return (true, null);

            try
            {
                var body = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
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