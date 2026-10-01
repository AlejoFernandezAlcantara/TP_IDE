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

        public async Task<bool> CrearAsync(PacienteDTO paciente)
        {
            var response = await _client.PostAsJsonAsync("pacientes", paciente);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(PacienteDTO paciente)
        {
            // El PUT del WebAPI es /api/pacientes (sin id en la URL)
            var response = await _client.PutAsJsonAsync("pacientes", paciente);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(int nroPaciente)
        {
            var response = await _client.DeleteAsync($"pacientes/{nroPaciente}");
            return response.IsSuccessStatusCode;
        }
    }
}