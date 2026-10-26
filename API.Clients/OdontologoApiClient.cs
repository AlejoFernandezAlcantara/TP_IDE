using System.Net.Http.Headers;
using System.Net.Http.Json;
using DTO;

namespace API.Clients
{
    public class OdontologoApiClient
    {
        private readonly HttpClient _client;

        public OdontologoApiClient(HttpClient client)
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

        public async Task<List<OdontologoDTO>?> GetAllAsync()
        {
            var response = await _client.GetAsync("odontologos");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<List<OdontologoDTO>>();
        }

        public async Task<OdontologoDTO?> GetByMatriculaAsync(string matricula)
        {
            var response = await _client.GetAsync($"odontologos/{matricula}");
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<OdontologoDTO>();
        }

        public async Task<(bool Ok, string? Error)> CrearAsync(OdontologoDTO odontologo)
        {
            var response = await _client.PostAsJsonAsync("odontologos", odontologo);
            return await ResultadoAsync(response, "No se pudo guardar el odontólogo.");
        }

        public async Task<(bool Ok, string? Error)> ActualizarAsync(OdontologoDTO odontologo)
        {
            var response = await _client.PutAsJsonAsync("odontologos", odontologo);
            return await ResultadoAsync(response, "No se pudo guardar el odontólogo.");
        }

        public async Task<bool> EliminarAsync(string matricula)
        {
            var response = await _client.DeleteAsync($"odontologos/{matricula}");
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