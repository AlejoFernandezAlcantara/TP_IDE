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

        public async Task<List<ReservaDTO>?> GetAllAsync()
        {
            var response = await _client.GetAsync("reservas");
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ReservaDTO>>();
        }

        public async Task<List<ReservaDTO>?> GetByPacienteAsync(int pacienteId)
        {
            var response = await _client.GetAsync($"reservas/paciente/{pacienteId}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ReservaDTO>>();
        }

        public async Task<bool> CrearAsync(ReservaDTO dto)
        {
            var response = await _client.PostAsJsonAsync("reservas", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(ReservaDTO dto)
        {
            var response = await _client.PutAsJsonAsync("reservas", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(int pacienteId, string odontologoMatricula, DateTime fechaCreacion)
        {
            // Convertir la fecha a formato ISO para la URL
            var fechaFormato = fechaCreacion.ToString("yyyy-MM-dd");
            var response = await _client.DeleteAsync($"reservas/{pacienteId}/{odontologoMatricula}/{fechaFormato}");
            return response.IsSuccessStatusCode;
        }
    }
}
