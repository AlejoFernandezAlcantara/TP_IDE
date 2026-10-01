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

        public async Task<List<TurnoDTO>?> GetAllAsync()
        {
            var response = await _client.GetAsync("turnos");
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<TurnoDTO>>();
        }

        public async Task<TurnoDTO?> GetByCodigoAsync(int codigo)
        {
            var response = await _client.GetAsync($"turnos/{codigo}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<TurnoDTO>();
        }

        public async Task<List<TurnoDTO>?> GetByOdontologoAsync(string matricula)
        {
            var response = await _client.GetAsync($"turnos/odontologo/{matricula}");
            
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<TurnoDTO>>();
        }

        public async Task<bool> CrearAsync(TurnoDTO dto)
        {
            var response = await _client.PostAsJsonAsync("turnos", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAsync(TurnoDTO dto)
        {
            var response = await _client.PutAsJsonAsync($"turnos/{dto.Codigo}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(int codigo)
        {
            var response = await _client.DeleteAsync($"turnos/{codigo}");
            return response.IsSuccessStatusCode;
        }
    }
}
